import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import { getGenresUseCase, createGenreUseCase, updateGenreUseCase, deleteGenreUseCase } from "../domain/UseCases/genreUseCase";

const GenresPage = () => {
    const [genres, setGenres] = useState([]);
    const [currentPage, setCurrentPage] = useState(1); // Trang hiện tại
    const [pageSize, setPageSize] = useState(10); // Số lượng item trên mỗi trang
    const [totalItems, setTotalItems] = useState(0); // Tổng số lượng item

    const fetchGenres = async () => {
        try {
            const data = await getGenresUseCase();
            setGenres(data);
            setTotalItems(data.length);
        } catch (error) {
            console.error("Failed to fetch genres:", error);
        }
    };
    useEffect(() => {
        fetchGenres();
    }, []);

    const handleAdd = async (data) => {
        try {
            const newGenre = await createGenreUseCase({ name: data.name });
            setGenres([...genres, newGenre]);
            await fetchGenres();
        } catch (error) {
            console.error("Failed to create genre:", error);
        }
    };

    const handleEdit = async (id, data) => {
        try {
            const updatedGenre = await updateGenreUseCase(id, { name: data.name });
            setGenres(genres.map((g) => (g.id === id ? updatedGenre : g)));
            await fetchGenres();
        } catch (error) {
            console.error("Failed to update genre:", error);
        }
    };

    const handleDelete = async (id) => {
        try {
            await deleteGenreUseCase(id);
            setGenres(genres.filter((g) => g.id !== id));
            await fetchGenres();
            setTotalItems(genres.length);

            const maxPage = Math.ceil(genres.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
        } catch (error) {
            console.error("Failed to delete genre:", error);
        }
    };
    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return genres.slice(startIndex, endIndex);
    };

    const columns = [
        { key: "id", label: "ID" },
        { key: "name", label: "Name" },
    ];

    const formFields = [{ key: "name", label: "Name" }];

    return (
        <CrudTable
            title="Genres"
            //data={genres}
            data={getPaginatedData()}
            columns={columns}
            formFields={formFields}
            onAdd={handleAdd}
            onEdit={handleEdit}
            onDelete={handleDelete}
            pagination={{
                current: currentPage, // Trang hiện tại
                pageSize: pageSize, // Số lượng item mỗi trang
                total: totalItems, // Tổng số item
                onChange: (page, pageSize) => {
                    setCurrentPage(page); // Cập nhật trang khi thay đổi
                    setPageSize(pageSize); // Cập nhật số lượng item mỗi trang
                },
                showSizeChanger: true, // Cho phép người dùng thay đổi số lượng item mỗi trang
                pageSizeOptions: ["5", "10", "15", "20"], // Các lựa chọn số lượng item mỗi trang
            }}
        />
    );
};

export default GenresPage;

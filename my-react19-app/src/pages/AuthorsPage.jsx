import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import { getAuthorsUseCase, createAuthorUseCase, updateAuthorUseCase, deleteAuthorUseCase } from "../domain/UseCases/authorUseCase";

const AuthorsPage = () => {
    const [authors, setAuthors] = useState([]);
    const [currentPage, setCurrentPage] = useState(1); // Trang hiện tại
    const [pageSize, setPageSize] = useState(10); // Số lượng item trên mỗi trang
    const [totalItems, setTotalItems] = useState(0); // Tổng số lượng item

    const fetchAuthors = async () => {
        try {
            const data = await getAuthorsUseCase();
            setAuthors(data);
            setTotalItems(data.length);
        } catch (error) {
            console.error("Failed to fetch authors:", error);
        }
    };
    useEffect(() => {
        fetchAuthors();
    }, []);

    const handleAdd = async (data) => {
        try {
            const newAuthor = await createAuthorUseCase({ name: data.name });
            setAuthors([...authors, newAuthor]);
            await fetchAuthors();
        } catch (error) {
            console.error("Failed to create author:", error);
        }
    };

    const handleEdit = async (id, data) => {
        try {
            const updatedAuthor = await updateAuthorUseCase(id, { name: data.name });
            setAuthors(authors.map((a) => (a.id === id ? updatedAuthor : a)));
            await fetchAuthors();
        } catch (error) {
            console.error("Failed to update author:", error);
        }
    };

    const handleDelete = async (id) => {
        try {
            await deleteAuthorUseCase(id);
            setAuthors(authors.filter((a) => a.id !== id));
            await fetchAuthors();
            const maxPage = Math.ceil(authors.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
        } catch (error) {
            console.error("Failed to delete author:", error);
        }
    };
    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return authors.slice(startIndex, endIndex);
    };

    const columns = [
        { key: "id", label: "ID" },
        { key: "name", label: "Name" },
    ];

    const formFields = [{ key: "name", label: "Name" }];

    return (
        <CrudTable
            title="Authors"
            //data={authors}
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

export default AuthorsPage;

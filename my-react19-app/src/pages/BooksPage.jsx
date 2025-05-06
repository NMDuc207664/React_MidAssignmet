import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import { getBooksUseCase, createBookUseCase, updateBookUseCase, deleteBookUseCase } from "../domain/UseCases/bookUseCase";
import { getAuthorsUseCase } from "../domain/UseCases/authorUseCase";
import { getGenresUseCase } from "../domain/UseCases/genreUseCase";

const BooksPage = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState([]);
    const [genres, setGenres] = useState([]);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1); // Trang hiện tại
    const [pageSize, setPageSize] = useState(10); // Số lượng item trên mỗi trang
    const [totalItems, setTotalItems] = useState(0); // Tổng số lượng item

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [booksData, authorsData, genresData] = await Promise.all([getBooksUseCase(), getAuthorsUseCase(), getGenresUseCase()]);
                setBooks(booksData);
                setTotalItems(booksData.length); // Tổng số lượng sách
                setAuthors(authorsData);
                setGenres(genresData);
            } catch (error) {
                setError(error.message);
            }
        };
        fetchData();
    }, []);

    const selectOptions = {
        bookAuthors: authors.map((a) => ({ value: a.id, label: a.name })),
        bookGenres: genres.map((g) => ({ value: g.id, label: g.name })),
    };

    const columns = [
        { key: "id", label: "ID" },
        { key: "name", label: "Tên" },
        { key: "description", label: "Mô tả" },
        { key: "storageNumber", label: "Số lượng tồn kho" },
        {
            key: "bookAuthors",
            label: "Tác giả",
            render: (item) =>
                item.bookAuthors
                    ?.map((ba) => authors.find((a) => a.id === ba.authorID)?.name)
                    .filter(Boolean)
                    .join(", ") || "—",
        },
        {
            key: "bookGenres",
            label: "Thể loại",
            render: (item) =>
                item.bookGenres
                    ?.map((bg) => genres.find((g) => g.id === bg.genreId)?.name)
                    .filter(Boolean)
                    .join(", ") || "—",
        },
    ];

    const formFields = [
        { key: "name", type: "text", label: "Tên" },
        { key: "description", type: "text", label: "Mô tả" },
        { key: "storageNumber", type: "number", label: "Số lượng tồn kho" },
        { key: "bookAuthors", type: "select", label: "Tác giả", multiple: true },
        { key: "bookGenres", type: "select", label: "Thể loại", multiple: true },
    ];

    const handleAdd = async (newBook) => {
        try {
            const payload = {
                name: newBook.name,
                description: newBook.description,
                storageNumber: parseInt(newBook.storageNumber),
                bookAuthors: (newBook.bookAuthors || []).map((id) => ({
                    authorID: id,
                })),
                bookGenres: (newBook.bookGenres || []).map((id) => ({
                    genreId: id,
                })),
            };
            await createBookUseCase(payload);
            const updated = await getBooksUseCase();
            setBooks(updated);
        } catch (error) {
            setError(error.message);
        }
    };

    const handleEdit = async (id, updatedBook) => {
        try {
            const payload = {
                name: updatedBook.name,
                description: updatedBook.description,
                storageNumber: parseInt(updatedBook.storageNumber),
                bookAuthors: (updatedBook.bookAuthors || []).map((id) => ({
                    authorID: id,
                })),
                bookGenres: (updatedBook.bookGenres || []).map((id) => ({
                    genreId: id,
                })),
            };
            await updateBookUseCase(id, payload);
            const updated = await getBooksUseCase();
            setBooks(updated);
        } catch (error) {
            setError(error.message);
        }
    };

    const handleDelete = async (id) => {
        try {
            await deleteBookUseCase(id);
            const updated = await getBooksUseCase();
            setBooks(updated);
            setTotalItems(updated.length);

            // Check if we need to adjust currentPage after deletion
            const maxPage = Math.ceil(updated.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
        } catch (error) {
            setError(error.message);
        }
    };
    // Calculate paginated data
    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return books.slice(startIndex, endIndex);
    };
    return (
        <CrudTable
            title="Books"
            // data={books}
            data={getPaginatedData()}
            columns={columns}
            formFields={formFields}
            onAdd={handleAdd}
            onEdit={handleEdit}
            onDelete={handleDelete}
            selectOptions={selectOptions}
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

export default BooksPage;

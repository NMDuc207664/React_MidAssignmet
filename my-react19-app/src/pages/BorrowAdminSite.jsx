import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import { getBorrowsUseCase, createBorrowUseCase, updateBorrowUseCase, deleteBorrowUseCase } from "../domain/UseCases/borrowUseCase";
import { getBooksUseCase } from "../domain/UseCases/bookUseCase";

const BorrowAdminSite = () => {
    const [borrows, setBorrows] = useState([]);
    const [books, setBooks] = useState([]);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [currentUser, setCurrentUser] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                // Lấy thông tin người dùng hiện tại
                const user = JSON.parse(localStorage.getItem("user")) || null;
                setCurrentUser(user);

                // Lấy danh sách sách từ database
                const booksData = await getBooksUseCase();
                setBooks(booksData);

                // Lấy danh sách các request mượn sách
                const borrowsData = await getBorrowsUseCase();
                setBorrows(borrowsData);
                setTotalItems(borrowsData.length);
            } catch (error) {
                setError(error.message);
                console.error("Failed to fetch data:", error);
            }
        };
        fetchData();
    }, []);

    const statusMap = {
        0: "Waiting",
        1: "Approved",
        2: "Rejected",
    };

    const selectOptions = {
        requestStatus: [
            { value: 0, label: "Waiting" },
            { value: 1, label: "Approved" },
            { value: 2, label: "Rejected" },
        ],
        bookIds: books.map((book) => ({
            value: book.id,
            label: `${book.name} (ID: ${book.id})`,
        })),
    };

    const columns = [
        { key: "id", label: "ID" },
        {
            key: "requestedDate",
            label: "Requested Date",
            render: (item) => new Date(item.requestedDate).toLocaleString(),
        },
        {
            key: "requestStatus",
            label: "Status",
            render: (item) => statusMap[item.requestStatus] || "Unknown",
        },
        { key: "fullName", label: "Full Name" },
        { key: "email", label: "Email" },
        { key: "phoneNumber", label: "Phone Number" },
        { key: "address", label: "Address" },
        {
            key: "borrowingDetails",
            label: "Books",
            render: (item) => item.borrowingDetails?.map((bd) => bd.bookName).join(", ") || "—",
        },
    ];

    const formFields = [
        {
            key: "userId",
            type: "text",
            label: "User ID",
            placeholder: "Leave empty for current user",
            required: false,
            visible: (mode) => mode === "create" || mode === "edit",
            disabled: (mode) => mode === "edit", // Read-only trong edit
        },
        {
            key: "bookIds",
            type: "select",
            label: "Books",
            multiple: true,
            required: true,
            visible: (mode) => mode === "create",
        },
        {
            key: "requestStatus",
            type: "select",
            label: "Status",
            required: true,
            visible: (mode) => mode === "edit",
        },
    ];

    const handleAdd = async (newBorrow) => {
        try {
            const userId = newBorrow.userId || (currentUser && currentUser.id);
            if (!userId) {
                throw new Error("User ID is required. Please log in or provide a User ID.");
            }
            if (!newBorrow.bookIds || newBorrow.bookIds.length === 0) {
                throw new Error("You must select at least one book.");
            }

            console.log("newBorrow.bookIds:", newBorrow.bookIds); // Log để kiểm tra bookIds
            console.log("Selected userId:", userId); // Log để kiểm tra userId

            const payload = {
                userId: userId,
                borrowingDetails: newBorrow.bookIds.map((bookId) => ({ bookId })),
            };

            await createBorrowUseCase(payload);

            // Refresh the data
            const booksData = await getBooksUseCase();
            setBooks(booksData);
            const borrowsData = await getBorrowsUseCase();
            setBorrows(borrowsData);
            setTotalItems(borrowsData.length);
            setError(null);
        } catch (error) {
            setError(error.message);
            console.error("handleAdd error:", error);
        }
    };

    const handleEdit = async (id, updatedBorrow) => {
        try {
            const status = parseInt(updatedBorrow.requestStatus);
            if (![0, 1, 2].includes(status)) {
                throw new Error("Invalid status value");
            }

            const payload = {
                requestStatus: status,
            };

            await updateBorrowUseCase(id, payload);

            // Refresh the data
            const booksData = await getBooksUseCase();
            setBooks(booksData);
            const borrowsData = await getBorrowsUseCase();
            setBorrows(borrowsData);
            setTotalItems(borrowsData.length);
            setError(null);
        } catch (error) {
            setError(error.message);
            console.error("handleEdit error:", error);
        }
    };

    const handleDelete = async (id) => {
        try {
            await deleteBorrowUseCase(id);

            // Refresh the data
            const booksData = await getBooksUseCase();
            setBooks(booksData);
            const borrowsData = await getBorrowsUseCase();
            setBorrows(borrowsData);
            setTotalItems(borrowsData.length);
            const maxPage = Math.ceil(borrowsData.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
        } catch (error) {
            setError(error.message);
            console.error("handleDelete error:", error);
        }
    };
    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return borrows.slice(startIndex, endIndex);
    };

    return (
        <div>
            {error && <div className="mb-4 rounded border border-red-400 bg-red-100 px-4 py-3 text-red-700">{error}</div>}
            <CrudTable
                title="Borrowing Requests"
                //data={borrows}
                data={getPaginatedData()}
                columns={columns}
                formFields={formFields}
                onAdd={handleAdd}
                onEdit={handleEdit}
                onDelete={handleDelete}
                selectOptions={selectOptions}
                pagination={{
                    current: currentPage,
                    pageSize: pageSize,
                    total: totalItems,
                    onChange: (page, pageSize) => {
                        setCurrentPage(page);
                        setPageSize(pageSize);
                    },
                    showSizeChanger: true,
                    pageSizeOptions: ["5", "10", "15", "20"],
                }}
            />
        </div>
    );
};

export default BorrowAdminSite;

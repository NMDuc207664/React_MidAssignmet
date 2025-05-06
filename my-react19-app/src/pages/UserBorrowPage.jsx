import React, { useState, useEffect } from "react";
import { getBooksUseCase } from "../domain/UseCases/bookUseCase";
import { getAuthorsUseCase } from "../domain/UseCases/authorUseCase";
import { getGenresUseCase } from "../domain/UseCases/genreUseCase";
import { createBorrowUseCase } from "../domain/UseCases/borrowUseCase";
import { accountRepository } from "../data/Repositories/accountRepository";
import { Pagination, Spin, Button, message } from "antd";
import { ShoppingCartOutlined, DeleteOutlined, SendOutlined, BookOutlined } from "@ant-design/icons";

const UserBorrowPage = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState([]);
    const [genres, setGenres] = useState([]);
    const [cart, setCart] = useState([]); // Giỏ hàng lưu bookIds
    const [error, setError] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [sidebarOpen, setSidebarOpen] = useState(false);

    const BOOKS_PER_PAGE = 12;

    useEffect(() => {
        const fetchData = async () => {
            try {
                setIsLoading(true);

                // Get user from localStorage using the accountRepository helper
                const user = accountRepository.getCurrentUser();
                console.log("Current user from accountRepository:", user);
                setCurrentUser(user);

                // Lấy danh sách sách, tác giả, thể loại
                const [booksData, authorsData, genresData] = await Promise.all([getBooksUseCase(), getAuthorsUseCase(), getGenresUseCase()]);

                setBooks(booksData);
                setAuthors(authorsData);
                setGenres(genresData);
            } catch (error) {
                console.error("Error fetching data:", error);
                setError(error.message);
                message.error("Failed to load books: " + error.message);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, []);

    // Update sidebar visibility when cart changes
    useEffect(() => {
        if (cart.length > 0) {
            setSidebarOpen(true);
        }
    }, [cart]);

    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
        // Scroll to top when changing page
        window.scrollTo(0, 0);
    };

    const handleAddToCart = (bookId) => {
        console.log("Adding to cart, current user:", currentUser);

        if (!currentUser) {
            setError("Please log in to add books to your request.");
            message.error("Please log in to add books to your request.");
            return;
        }

        if (currentUser.role === "User" && cart.length >= 5) {
            setError("You cannot request more than 5 books at a time.");
            message.warning("You cannot request more than 5 books at a time.");
            return;
        }

        if (cart.includes(bookId)) {
            setError("This book is already in your request.");
            message.info("This book is already in your request.");
            return;
        }

        setCart([...cart, bookId]);
        setError(null);
        message.success("Book added to your request");
    };

    const handleRemoveFromCart = (bookId) => {
        setCart(cart.filter((id) => id !== bookId));
        setError(null);
        message.info("Book removed from your request");

        // Close sidebar if cart becomes empty
        if (cart.length <= 1) {
            setSidebarOpen(false);
        }
    };

    const handleRequest = async () => {
        if (!currentUser) {
            setError("Please log in to submit a request.");
            message.error("Please log in to submit a request.");
            return;
        }

        if (cart.length === 0) {
            setError("Please add at least one book to your request.");
            message.warning("Please add at least one book to your request.");
            return;
        }

        try {
            const payload = {
                userId: currentUser.id,
                borrowingDetails: cart.map((bookId) => ({ bookId })),
            };

            console.log("Request payload:", payload);

            await createBorrowUseCase(payload);
            setCart([]); // Xóa giỏ hàng sau khi gửi yêu cầu
            setError(null);
            message.success("Request submitted successfully!");
            setSidebarOpen(false);

            // Refetch books to update stock numbers
            const updatedBooks = await getBooksUseCase();
            setBooks(updatedBooks);
        } catch (error) {
            setError(error.message);
            message.error("Failed to submit request: " + error.message);
            console.error("handleRequest error:", error);
        }
    };

    const renderAuthors = (bookAuthors) =>
        bookAuthors
            ?.map((ba) => authors.find((a) => a.id === ba.authorID)?.name)
            .filter(Boolean)
            .join(", ") || "—";

    const renderGenres = (bookGenres) =>
        bookGenres
            ?.map((bg) => genres.find((g) => g.id === bg.genreId)?.name)
            .filter(Boolean)
            .join(", ") || "—";

    // Calculate current books to display based on pagination
    const indexOfLastBook = currentPage * BOOKS_PER_PAGE;
    const indexOfFirstBook = indexOfLastBook - BOOKS_PER_PAGE;
    const currentBooks = books.slice(indexOfFirstBook, indexOfLastBook);

    if (isLoading) {
        return (
            <div className="flex h-screen items-center justify-center">
                <Spin
                    size="large"
                    tip="Loading books..."
                />
            </div>
        );
    }

    return (
        <div className="relative flex min-h-screen">
            {/* Main Content */}
            <div className={`flex-1 p-6 transition-all duration-300 ${sidebarOpen ? "pr-80" : "pr-6"}`}>
                {error && <div className="mb-4 rounded border border-red-400 bg-red-100 px-4 py-3 text-red-700">{error}</div>}

                {!currentUser && (
                    <div className="mb-4 rounded border border-yellow-400 bg-yellow-100 px-4 py-3 text-yellow-700">
                        You are not logged in. Please log in to request books.
                    </div>
                )}

                {currentUser && (
                    <div className="mb-4 rounded border border-green-400 bg-green-100 px-4 py-3 text-green-700">
                        Logged in as: {currentUser.email || currentUser.username || currentUser.name || "User"}
                        {currentUser.role && ` (${currentUser.role})`}
                    </div>
                )}

                <div className="mb-6 flex items-center justify-between">
                    <h2 className="text-2xl font-bold">Available Books</h2>
                    {cart.length > 0 && !sidebarOpen && (
                        <Button
                            type="primary"
                            icon={<ShoppingCartOutlined />}
                            onClick={() => setSidebarOpen(true)}
                            className="bg-blue-600 hover:bg-blue-700"
                        >
                            View Request ({cart.length})
                        </Button>
                    )}
                </div>

                <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                    {currentBooks.map((book) => (
                        <div
                            key={book.id}
                            className="rounded-lg border border-gray-200 bg-white p-4 shadow-md transition hover:shadow-lg"
                        >
                            <div className="flex h-full flex-col justify-between">
                                <div>
                                    <div className="mb-2 flex items-center">
                                        <BookOutlined className="mr-2 text-blue-600" />
                                        <h3 className="text-lg font-semibold">{book.name}</h3>
                                    </div>

                                    <p className="mb-3 text-gray-600">{book.description}</p>

                                    <div className="mb-4 space-y-1 text-sm">
                                        <p className="text-gray-700">
                                            <span className="font-medium">Stock:</span> {book.storageNumber}
                                        </p>
                                        <p className="text-gray-700">
                                            <span className="font-medium">Authors:</span> {renderAuthors(book.bookAuthors)}
                                        </p>
                                        <p className="text-gray-700">
                                            <span className="font-medium">Genres:</span> {renderGenres(book.bookGenres)}
                                        </p>
                                    </div>
                                </div>

                                <Button
                                    type={cart.includes(book.id) ? "default" : "primary"}
                                    icon={<ShoppingCartOutlined />}
                                    className={`mt-2 w-full ${
                                        cart.includes(book.id) || book.storageNumber === 0
                                            ? "border-gray-300 bg-gray-100 text-gray-500"
                                            : currentUser
                                              ? "border-blue-600 bg-blue-600 text-white hover:bg-blue-700"
                                              : "border-gray-300 bg-gray-100 text-gray-500"
                                    }`}
                                    onClick={() => handleAddToCart(book.id)}
                                    disabled={cart.includes(book.id) || !currentUser || book.storageNumber === 0}
                                >
                                    {cart.includes(book.id)
                                        ? "Added to Request"
                                        : book.storageNumber === 0
                                          ? "Out of Stock"
                                          : currentUser
                                            ? "Add to Request"
                                            : "Login Required"}
                                </Button>
                            </div>
                        </div>
                    ))}
                </div>

                {/* Pagination */}
                <div className="mt-8 flex justify-center">
                    <Pagination
                        current={currentPage}
                        total={books.length}
                        pageSize={BOOKS_PER_PAGE}
                        onChange={handlePageChange}
                        showSizeChanger={false}
                    />
                </div>
            </div>

            {/* Sidebar for cart */}
            <div
                className={`fixed top-0 right-0 h-full w-72 bg-white shadow-lg transition-transform duration-300 ${
                    sidebarOpen ? "translate-x-0" : "translate-x-full"
                }`}
                style={{ zIndex: 1000 }}
            >
                <div className="flex h-full flex-col p-4">
                    <div className="mb-4 flex items-center justify-between border-b pb-2">
                        <h3 className="text-xl font-semibold">Your Request</h3>
                        <Button
                            type="text"
                            icon={<DeleteOutlined />}
                            onClick={() => setSidebarOpen(false)}
                            className="text-gray-600 hover:text-red-600"
                        />
                    </div>

                    <div className="flex-1 overflow-y-auto">
                        {cart.length === 0 ? (
                            <p className="text-gray-600">No books selected.</p>
                        ) : (
                            <ul className="space-y-2">
                                {cart.map((bookId) => {
                                    const book = books.find((b) => b.id === bookId);
                                    return (
                                        <li
                                            key={bookId}
                                            className="rounded-lg border border-gray-200 bg-gray-50 p-2"
                                        >
                                            <div className="flex items-center justify-between">
                                                <span className="flex-1 font-medium">{book ? book.name : "Unknown Book"}</span>
                                                <Button
                                                    type="text"
                                                    icon={<DeleteOutlined />}
                                                    danger
                                                    onClick={() => handleRemoveFromCart(bookId)}
                                                    className="flex items-center justify-center"
                                                />
                                            </div>
                                        </li>
                                    );
                                })}
                            </ul>
                        )}
                    </div>

                    <div className="border-t pt-4">
                        <div className="mb-2 text-sm text-gray-600">
                            {cart.length} book{cart.length !== 1 ? "s" : ""} selected
                        </div>
                        <Button
                            type="primary"
                            icon={<SendOutlined />}
                            className={`w-full ${cart.length > 0 ? "bg-blue-600 hover:bg-blue-700" : "bg-gray-300"}`}
                            onClick={handleRequest}
                            disabled={cart.length === 0}
                        >
                            Submit Request
                        </Button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default UserBorrowPage;

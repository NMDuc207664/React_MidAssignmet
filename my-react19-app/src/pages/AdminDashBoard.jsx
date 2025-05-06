import { useState } from "react";
import BooksPage from "./BooksPage";
import AuthorsPage from "./AuthorsPage";
import GenresPage from "./GenresPage";
import BorrowAdminSite from "./BorrowAdminSite";
import UsersPage from "./UsersPage";
import RecordsPage from "./RecordsPage";
const AdminDashboard = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const [selectedPage, setSelectedPage] = useState("books");

    const toggleSidebar = () => {
        setIsSidebarOpen(!isSidebarOpen);
    };
    const renderContent = () => {
        switch (selectedPage) {
            case "books":
                return <BooksPage />;
            case "users":
                return <UsersPage />;
            case "authors":
                return <AuthorsPage />;
            case "genres":
                return <GenresPage />;
            case "requests":
                return <BorrowAdminSite />;
            case "records":
                return <RecordsPage />;
            default:
                return <div>Select a page</div>;
        }
    };

    return (
        <div className="flex h-screen bg-gray-100">
            {/* Sidebar */}
            <div className={`bg-gray-800 text-white transition-all duration-300 ${isSidebarOpen ? "w-64" : "w-16"}`}>
                <div className="flex items-center justify-between p-4">
                    {isSidebarOpen && <h1 className="text-xl font-bold">Admin Panel</h1>}
                    <button
                        onClick={toggleSidebar}
                        className="rounded p-2 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "<" : ">"}
                    </button>
                </div>
                <nav className="mt-4">
                    <button
                        onClick={() => setSelectedPage("books")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Books" : "B"}
                    </button>
                    <button
                        onClick={() => setSelectedPage("users")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Users" : "U"}
                    </button>
                    <button
                        onClick={() => setSelectedPage("authors")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Authors" : "A"}
                    </button>
                    <button
                        onClick={() => setSelectedPage("genres")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Genres" : "G"}
                    </button>
                    <button
                        onClick={() => setSelectedPage("requests")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Requests" : "R"}
                    </button>
                    <button
                        onClick={() => setSelectedPage("records")}
                        className="flex w-full items-center p-4 hover:bg-gray-700"
                    >
                        {isSidebarOpen ? "Records" : "R"}
                    </button>
                </nav>
            </div>

            {/* Main Content */}
            <div className="flex-1 overflow-auto p-8">{renderContent()}</div>
        </div>
    );
};

export default AdminDashboard;

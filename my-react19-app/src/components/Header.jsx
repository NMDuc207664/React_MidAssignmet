import React, { useState } from "react";
import { AppstoreOutlined, InfoCircleOutlined, DashboardOutlined, DownOutlined, BookOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./hooks/useAuth";

export default function Header() {
    const navigate = useNavigate();
    const { user, logout, isAdmin } = useAuth();
    const [userMenuOpen, setUserMenuOpen] = useState(false);

    console.log("Header: Current User:", user); // Debug

    const fullName = user ? `${user.firstName || ""} ${user.lastName || ""}`.trim() || user.username || user.email || "Guest" : "Guest";

    const onClickMenu = (key) => {
        setUserMenuOpen(false);
        if (key === "home") navigate("/home");
        else if (key === "about") navigate("/about");
        else if (key === "dashboard") navigate("/dashboard");
    };

    const onClickUserMenu = (key) => {
        setUserMenuOpen(false);
        if (key === "profile") {
            if (user?.id) {
                console.log("Header: Navigating to profile for user ID:", user.id); // Debug
                navigate("/profile");
            } else {
                console.error("Header: No valid user session or ID, redirecting to login"); // Debug
                navigate("/login");
            }
        } else if (key === "history") {
            navigate("/history");
        } else if (key === "logout") {
            console.log("Header: Logging out user"); // Debug
            logout();
            navigate("/login");
        }
    };

    const toggleUserMenu = () => {
        setUserMenuOpen((prev) => !prev);
    };

    return (
        <header
            className="flex items-center bg-white px-4 shadow-md"
            style={{ height: "60px" }}
        >
            {/* Logo */}
            <div
                className="mr-8 cursor-pointer text-xl font-bold select-none"
                onClick={() => onClickMenu("home")}
            >
                <BookOutlined style={{ fontSize: 22 }} />
                MY LIBRARY
            </div>

            {/* Navigation Menu */}
            <nav className="flex items-center gap-8 text-gray-700">
                <button
                    onClick={() => onClickMenu("home")}
                    className="flex items-center gap-1 hover:text-blue-600 focus:outline-none"
                    type="button"
                >
                    <AppstoreOutlined style={{ fontSize: 18 }} />
                    <span className="text-sm font-medium">Home</span>
                </button>
                <button
                    onClick={() => onClickMenu("about")}
                    className="flex items-center gap-1 hover:text-blue-600 focus:outline-none"
                    type="button"
                >
                    <InfoCircleOutlined style={{ fontSize: 18 }} />
                    <span className="text-sm font-medium">About</span>
                </button>

                {/* Dashboard button - only visible to admins */}
                {isAdmin() && (
                    <button
                        onClick={() => onClickMenu("dashboard")}
                        className="flex items-center gap-1 hover:text-blue-600 focus:outline-none"
                        type="button"
                    >
                        <DashboardOutlined style={{ fontSize: 18 }} />
                        <span className="text-sm font-medium">Dashboard</span>
                    </button>
                )}
            </nav>

            {/* User Section */}
            <div className="relative ml-auto">
                {user ? (
                    <>
                        <button
                            onClick={toggleUserMenu}
                            className="flex items-center space-x-1 text-gray-700 hover:text-blue-600 focus:outline-none"
                            type="button"
                            aria-haspopup="true"
                            aria-expanded={userMenuOpen}
                            aria-controls="usermenu"
                        >
                            <span className="text-sm font-medium select-none">{fullName}</span>
                            <DownOutlined style={{ fontSize: 12 }} />
                        </button>

                        {userMenuOpen && (
                            <div
                                id="usermenu"
                                className="absolute right-0 z-50 mt-2 w-48 rounded-md border border-gray-200 bg-white shadow-lg"
                                role="menu"
                            >
                                <button
                                    onClick={() => onClickUserMenu("profile")}
                                    className="w-full px-4 py-2 text-left text-sm hover:bg-blue-600 hover:text-white"
                                    role="menuitem"
                                    type="button"
                                >
                                    Profile
                                </button>
                                <button
                                    onClick={() => onClickUserMenu("history")}
                                    className="w-full px-4 py-2 text-left text-sm hover:bg-blue-600 hover:text-white"
                                    role="menuitem"
                                    type="button"
                                >
                                    Lịch sử request
                                </button>
                                <button
                                    onClick={() => onClickUserMenu("logout")}
                                    className="w-full px-4 py-2 text-left text-sm text-red-600 hover:bg-blue-600 hover:text-white"
                                    role="menuitem"
                                    type="button"
                                >
                                    Đăng xuất
                                </button>
                            </div>
                        )}
                    </>
                ) : (
                    <div className="flex items-center gap-4">
                        <button
                            onClick={() => navigate("/login")}
                            className="text-sm font-medium text-gray-700 hover:text-blue-600 focus:outline-none"
                            type="button"
                        >
                            Đăng nhập
                        </button>
                        <button
                            onClick={() => navigate("/register")}
                            className="text-sm font-medium text-gray-700 hover:text-blue-600 focus:outline-none"
                            type="button"
                        >
                            Đăng ký
                        </button>
                    </div>
                )}
            </div>
        </header>
    );
}

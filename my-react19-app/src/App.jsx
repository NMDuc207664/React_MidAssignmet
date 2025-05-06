import AdminDashboard from "./pages/AdminDashBoard";
import About from "./Pages/About";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import Footer from "./components/Footer";
import AuthorsPage from "./pages/AuthorsPage";
import BooksPage from "./pages/BooksPage";
import GenresPage from "./pages/GenresPage";
import RegisterPage from "./pages/RegisterPage";
import UserBorrowPage from "./pages/UserBorrowPage";
import ProfilePage from "./pages/ProfilePage";
import RequestHistoryPage from "./pages/RequestHistoryPage";
import { AuthProvider, useAuth, ProtectedRoute } from "./components/hooks/useAuth";
//import { AuthProvider, useAuth, ProtectedRoute } from "./hooks/useAuth";
import UsersPage from "./pages/UsersPage";
import RecordsPage from "./pages/RecordsPage";
import BorrowAdminSite from "./pages/BorrowAdminSite";

function AppRoutes() {
    return (
        <Routes>
            {/* Public routes */}
            <Route
                path="/login"
                element={<LoginPage />}
            />
            <Route
                path="/register"
                element={<RegisterPage />}
            />
            <Route
                path="/about"
                element={<About />}
            />

            {/* Protected routes */}
            <Route
                path="/profile"
                element={
                    <ProtectedRoute>
                        <ProfilePage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/history"
                element={
                    <ProtectedRoute>
                        <RequestHistoryPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/home"
                element={
                    <ProtectedRoute>
                        <UserBorrowPage />
                    </ProtectedRoute>
                }
            />

            {/* Admin-only routes */}
            <Route
                path="/dashboard"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <AdminDashboard />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/authors"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <AuthorsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/books"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <BooksPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/genres"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <GenresPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/users"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <UsersPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/records"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <RecordsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/requests"
                element={
                    <ProtectedRoute adminOnly={true}>
                        <BorrowAdminSite />
                    </ProtectedRoute>
                }
            />

            {/* Redirect to home or login based on auth status */}
            <Route
                path="/"
                element={
                    <Navigate
                        to="/home"
                        replace
                    />
                }
            />

            {/* Catch-all route */}
            <Route
                path="*"
                element={
                    <Navigate
                        to="/home"
                        replace
                    />
                }
            />
        </Routes>
    );
}

function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <div className="flex min-h-screen flex-col bg-gray-100">
                    <Header />
                    <div className="container mx-auto flex-1 p-4">
                        <AppRoutes />
                    </div>
                    <Footer />
                </div>
            </AuthProvider>
        </BrowserRouter>
    );
}

export default App;

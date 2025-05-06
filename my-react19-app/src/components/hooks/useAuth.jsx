import { useState, useEffect, createContext, useContext } from "react";
import { accountRepository } from "../../data/Repositories/accountRepository";
import { useNavigate } from "react-router-dom";

// Create Authentication Context
const AuthContext = createContext(null);

// Auth Provider component
export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Get current user from localStorage on component mount
        const loadUser = () => {
            const currentUser = accountRepository.getCurrentUser();
            setUser(currentUser);
            setLoading(false);
        };

        loadUser();

        // Listen for storage events (logout from another tab)
        const handleStorageChange = (e) => {
            if (e.key === "auth_token" || e.key === "user") {
                loadUser();
            }
        };

        window.addEventListener("storage", handleStorageChange);
        return () => window.removeEventListener("storage", handleStorageChange);
    }, []);

    // Auth functions
    const login = async (credentials) => {
        const response = await accountRepository.login(credentials);
        setUser(accountRepository.getCurrentUser());
        return response;
    };

    const logout = () => {
        accountRepository.logout();
        setUser(null);
    };

    const isAdmin = () => {
        return user?.role === "Admin";
    };

    const isAuthenticated = () => {
        return !!user && accountRepository.isLoggedIn();
    };

    return <AuthContext.Provider value={{ user, login, logout, isAdmin, isAuthenticated, loading }}>{children}</AuthContext.Provider>;
};

// Custom hook to use the auth context
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};

// Protected Route component for admin-only routes
export const ProtectedRoute = ({ children, adminOnly = false }) => {
    const { isAuthenticated, isAdmin, loading } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!loading) {
            if (!isAuthenticated()) {
                navigate("/login");
            } else if (adminOnly && !isAdmin()) {
                navigate("/home");
            }
        }
    }, [isAuthenticated, isAdmin, loading, navigate, adminOnly]);

    if (loading) {
        return <div className="flex h-64 items-center justify-center">Loading...</div>;
    }

    // If adminOnly is true, only allow admin users
    if (adminOnly && !isAdmin()) {
        return null;
    }

    // Authenticated users can access non-admin routes
    if (isAuthenticated()) {
        return children;
    }

    return null;
};

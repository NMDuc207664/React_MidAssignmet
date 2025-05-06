import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../components/hooks/useAuth";

function LoginPage() {
    const [formData, setFormData] = useState({
        username: "",
        password: "",
    });
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const { login } = useAuth(); // Sử dụng hook useAuth

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!formData.username || !formData.password) {
            setError("All fields are required");
            return;
        }
        setLoading(true);
        setError("");
        try {
            // Sử dụng login từ context thay vì gọi trực tiếp loginUseCase
            const response = await login(formData);

            if (response.isAuthSucessful && response.user) {
                console.log("Login successful. User:", response.user);

                // Điều hướng về trang chính sau khi đăng nhập thành công
                navigate("/home");
            } else {
                setError("Login failed: " + (response.errorMessage || "Unknown error"));
            }
        } catch (err) {
            setError(err.message || "Login failed");
            console.error("Login error:", err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="flex min-h-screen items-center justify-center bg-gray-100">
            <div className="w-full max-w-md rounded-lg bg-white p-8 shadow-lg">
                <h2 className="mb-6 text-center text-2xl font-bold">Login</h2>
                {error && <p className="mb-4 text-red-500">{error}</p>}
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label
                            className="mb-2 block text-gray-700"
                            htmlFor="username"
                        >
                            Username or Email
                        </label>
                        <input
                            type="text"
                            id="username"
                            name="username"
                            value={formData.username}
                            onChange={handleChange}
                            className="w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                            placeholder="Enter username or email"
                        />
                    </div>
                    <div className="mb-6">
                        <label
                            className="mb-2 block text-gray-700"
                            htmlFor="password"
                        >
                            Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleChange}
                            className="w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                            placeholder="Enter password"
                        />
                    </div>
                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full rounded-lg border border-blue-500 bg-white py-2 text-blue-500 transition-opacity hover:opacity-80 disabled:border-gray-300 disabled:bg-gray-100 disabled:text-gray-400"
                    >
                        {loading ? "Logging in..." : "Login"}
                    </button>
                </form>
                <p className="mt-4 text-center">
                    Don't have an account?{" "}
                    <Link
                        to="/register"
                        className="text-blue-500 hover:underline"
                    >
                        Register
                    </Link>
                </p>
            </div>
        </div>
    );
}

export default LoginPage;

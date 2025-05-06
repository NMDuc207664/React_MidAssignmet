import api from "../api";

// Helper function to decode JWT token
function decodeJwt(token) {
    try {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split("")
                .map(function (c) {
                    return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
                })
                .join(""),
        );
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error("Error decoding JWT:", error);
        return null;
    }
}

export const accountRepository = {
    async register(userData) {
        try {
            const response = await api.post("/accounts/register", userData);
            return response.data;
        } catch (error) {
            if (error.response && error.response.data) {
                throw error.response.data;
            }
            throw new Error("Registration failed");
        }
    },

    async login(credentials) {
        try {
            const response = await api.post("/accounts/login", credentials);
            console.log("Login response:", response.data); // Debug

            // Store the complete user information
            let userData;
            if (response.data.user) {
                userData = response.data.user;
                localStorage.setItem("user", JSON.stringify(userData));
                console.log("User data stored in localStorage:", userData);
            } else if (response.data.isAuthSucessful) {
                // Decode JWT to extract user ID, username, and role
                const token = response.data.token || response.data.accessToken;
                let decodedToken = null;
                if (token) {
                    decodedToken = decodeJwt(token);
                    console.log("Decoded JWT:", decodedToken); // Debug
                } else {
                    throw new Error("No token found in login response");
                }

                if (!decodedToken) {
                    throw new Error("Failed to decode JWT token");
                }

                // Map JWT claims to userData
                userData = {
                    id: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                    username: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || credentials.username,
                    email: response.data.email || credentials.username,
                    role: decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "User",
                    firstName: response.data.firstName || "",
                    lastName: response.data.lastName || "",
                    address: response.data.address || "",
                    phoneNumber: response.data.phoneNumber || "",
                };

                if (!userData.id) {
                    console.error("No user ID found in JWT claims:", decodedToken);
                    throw new Error("User ID is missing in JWT token");
                }

                localStorage.setItem("user", JSON.stringify(userData));
                console.log("Generated user data stored in localStorage:", userData);
                response.data.user = userData;
            } else {
                throw new Error("No user data found in login response");
            }

            // Store the token
            const token = response.data.token || response.data.accessToken;
            if (token) {
                localStorage.setItem("auth_token", token);
                console.log("Token stored in localStorage:", token);
            } else {
                console.warn("No token found in login response:", response.data);
                throw new Error("No token found in login response");
            }

            return response.data;
        } catch (error) {
            console.error("Login error:", error);
            if (error.response && error.response.data) {
                throw error.response.data;
            }
            throw new Error("Login failed");
        }
    },

    getCurrentUser() {
        try {
            const userJson = localStorage.getItem("user");
            if (userJson) {
                const user = JSON.parse(userJson);
                console.log("getCurrentUser:", user); // Debug
                return user;
            }
            console.warn("No user found in localStorage");
            return null;
        } catch (error) {
            console.error("Error getting current user:", error);
            return null;
        }
    },

    isLoggedIn() {
        const isLogged = !!localStorage.getItem("user") && !!localStorage.getItem("auth_token");
        console.log("isLoggedIn:", isLogged); // Debug
        return isLogged;
    },

    logout() {
        console.log("Logging out: Removing user and auth_token from localStorage"); // Debug
        localStorage.removeItem("user");
        localStorage.removeItem("auth_token");
    },
};

// src/api.js
import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5074/api", // Chỉ đến /api, KHÔNG thêm /accounts
    headers: {
        "Content-Type": "application/json",
    },
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("auth_token");
        console.log("Using token:", token); // debug
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error),
);

export default api;

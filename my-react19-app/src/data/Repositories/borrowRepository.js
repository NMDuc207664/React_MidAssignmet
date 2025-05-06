import api from "../api";

export const fetchBorrows = async () => {
    try {
        const response = await api.get("/request");
        return response.data;
    } catch (error) {
        console.error("fetchBorrows error:", error);
        throw error;
    }
};

export const fetchBorrowById = async (id) => {
    try {
        const response = await api.get(`/request/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowById error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchBorrowsByUserId = async (userId) => {
    try {
        const response = await api.get(`/request/by-user/${userId}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsByUserId error for User ID ${userId}:`, error);
        throw error;
    }
};

export const fetchBorrowsDelete = async (id) => {
    try {
        const response = await api.delete(`/request/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsDelete error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchBorrowsStatus = async (id, borrowData) => {
    try {
        const response = await api.put(`/request/${id}`, {
            requestStatus: borrowData.requestStatus,
        });
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsStatus error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchBorrowCreate = async (borrowData) => {
    try {
        const response = await api.post("/request", {
            UserId: borrowData.userId,
            borrowingDetails: borrowData.borrowingDetails.map((detail) => ({
                BookId: detail.bookId,
            })),
        });
        return response.data;
    } catch (error) {
        console.error("fetchBorrowCreate error:", error);
        throw error;
    }
};

import api from "../api";

export const fetchUsers = async () => {
    try {
        const response = await api.get("/user");
        return response.data;
    } catch (error) {
        console.error("fetchUsers error:", error);
        throw error;
    }
};

export const fetchUserById = async (id) => {
    try {
        const response = await api.get(`/user/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowById error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchUserDelete = async (id) => {
    try {
        const response = await api.delete(`/user/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsDelete error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchUserUpdateRole = async (id, roleData) => {
    try {
        const response = await api.put(`/user/${id}/role`, {
            role: roleData.role,
        });
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsStatus error for ID ${id}:`, error);
        throw error;
    }
};
export const fetchUserUpdateProfile = async (id, profileData) => {
    try {
        const response = await api.put(`/user/${id}/profile`, {
            firstName: profileData.firstName,
            lastName: profileData.lastName,
            address: profileData.address,
            phoneNumber: profileData.phoneNumber,
            email: profileData.email,
        });
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsStatus error for ID ${id}:`, error);
        throw error;
    }
};
export const fetchChangePassword = async (id, passwordData) => {
    try {
        const response = await api.put(`/user/${id}/change-password`, {
            currentPassword: passwordData.currentPassword,
            newPassword: passwordData.newPassword,
            confirmPassword: passwordData.confirmPassword,
        });
        return response.data;
    } catch (error) {
        console.error(`fetchBorrowsStatus error for ID ${id}:`, error);
        throw error;
    }
};
export const fetchResetPassword = async (id) => {
    try {
        const response = await api.put(`/user/${id}/reset-password`);
        return response.data;
    } catch (error) {
        console.error(`fetchResetPassword error for ID ${id}:`, error);
        throw error;
    }
};

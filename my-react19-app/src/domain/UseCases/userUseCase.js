import {
    fetchUsers,
    fetchUserById,
    fetchUserDelete,
    fetchUserUpdateRole,
    fetchUserUpdateProfile,
    fetchChangePassword,
    fetchResetPassword,
} from "../../data/Repositories/userRepository";
export const getUsersUseCase = async () => {
    try {
        return await fetchUsers();
    } catch (error) {
        console.error("getUsersUseCase error:", error);
        throw error;
    }
};
export const getUserByIdUseCase = async (id) => {
    try {
        return await fetchUserById(id);
    } catch (error) {
        console.error(`getUserByIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};
export const deleteUserUseCase = async (id) => {
    try {
        return await fetchUserDelete(id);
    } catch (error) {
        console.error(`deleteUserUseCase error for ID ${id}:`, error);
        throw error;
    }
};
export const updateUserRoleUseCase = async (id, roleData) => {
    try {
        return await fetchUserUpdateRole(id, roleData);
    } catch (error) {
        console.error(`updateUserRoleUseCase error for ID ${id}:`, error);
        throw error;
    }
};
export const updateUserProfileUseCase = async (id, profileData) => {
    try {
        return await fetchUserUpdateProfile(id, profileData);
    } catch (error) {
        console.error(`updateUserProfileUseCase error for ID ${id}:`, error);
        throw error;
    }
};
export const changePasswordUseCase = async (id, passwordData) => {
    try {
        return await fetchChangePassword(id, passwordData);
    } catch (error) {
        console.error(`changePasswordUseCase error for ID ${id}:`, error);
        throw error;
    }
};
export const resetPasswordUseCase = async (id) => {
    try {
        return await fetchResetPassword(id);
    } catch (error) {
        console.error(`resetPasswordUseCase error for ID ${id}:`, error);
        throw error;
    }
};

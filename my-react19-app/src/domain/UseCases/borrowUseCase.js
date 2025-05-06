import {
    fetchBorrows,
    fetchBorrowById,
    fetchBorrowCreate,
    fetchBorrowsStatus,
    fetchBorrowsDelete,
    fetchBorrowsByUserId,
} from "../../data/Repositories/borrowRepository";

export const getBorrowsUseCase = async () => {
    try {
        return await fetchBorrows();
    } catch (error) {
        console.error("getBorrowsUseCase error:", error);
        throw error;
    }
};

export const getBorrowByIdUseCase = async (id) => {
    try {
        return await fetchBorrowById(id);
    } catch (error) {
        console.error(`getBorrowByIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const createBorrowUseCase = async (borrowData) => {
    try {
        return await fetchBorrowCreate(borrowData);
    } catch (error) {
        console.error("createBorrowUseCase error:", error);
        throw error;
    }
};

export const updateBorrowUseCase = async (id, borrowData) => {
    try {
        return await fetchBorrowsStatus(id, borrowData);
    } catch (error) {
        console.error(`updateBorrowUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const deleteBorrowUseCase = async (id) => {
    try {
        return await fetchBorrowsDelete(id);
    } catch (error) {
        console.error(`deleteBorrowUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const getBorrowsByUserIdUseCase = async (id) => {
    try {
        return await fetchBorrowsByUserId(id);
    } catch (error) {
        console.error(`getBorrowsByUserIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};

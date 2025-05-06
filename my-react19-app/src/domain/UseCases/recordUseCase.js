import {
    fetchRecords,
    fetchRecordById,
    fetchRecordByBorrowingRequestId,
    deleteRecord,
    markBookPickedUp,
    markBookReturned,
    updateRecordStatusByDay,
} from "../../data/Repositories/recordRepository";

export const getRecordsUseCase = async () => {
    try {
        const records = await fetchRecords();
        return records;
    } catch (error) {
        console.error("getRecordsUseCase error:", error);
        throw error;
    }
};

export const getRecordByIdUseCase = async (id) => {
    try {
        const record = await fetchRecordById(id);
        return record;
    } catch (error) {
        console.error(`getRecordByIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const getRecordByBorrowingRequestIdUseCase = async (id) => {
    try {
        const record = await fetchRecordByBorrowingRequestId(id);
        return record;
    } catch (error) {
        console.error(`getRecordByBorrowingRequestIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const deleteRecordUseCase = async (id) => {
    try {
        await deleteRecord(id);
    } catch (error) {
        console.error(`deleteRecordUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const markBookPickedUpUseCase = async (id, adminId) => {
    try {
        const updatedRecord = await markBookPickedUp(id, adminId);
        return updatedRecord;
    } catch (error) {
        console.error(`markBookPickedUpUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const markBookReturnedUseCase = async (id, adminId) => {
    try {
        const updatedRecord = await markBookReturned(id, adminId);
        return updatedRecord;
    } catch (error) {
        console.error(`markBookReturnedUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const updateRecordStatusByDayUseCase = async (currentTime) => {
    try {
        await updateRecordStatusByDay(currentTime);
    } catch (error) {
        console.error("updateRecordStatusByDayUseCase error:", error);
        throw error;
    }
};

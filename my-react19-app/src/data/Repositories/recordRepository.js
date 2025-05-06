// import api from "../api";

// export const fetchRecords = async () => {
//     const response = await api.get("/record");
//     return response.data;
// };

// export const fetchRecordById = async (id) => {
//     const response = await api.get(`/record/${id}`);
//     return response.data;
// };

// export const fetchRecordByBorrowingRequestId = async (id) => {
//     const response = await api.get(`/record/by-request/${id}`);
//     return response.data;
// };

// export const deleteRecord = async (id) => {
//     await api.delete(`/record/${id}`);
// };

// export const markBookPickedUp = async (id, adminId) => {
//     const response = await api.put(`/record/pickup/${id}`, {
//         Id: adminId.id,
//     });
//     return response.data;
// };

// export const markBookReturned = async (id, adminId) => {
//     const response = await api.put(`/record/return/${id}`, {
//         Id: adminId.id,
//     });
//     return response.data;
// };

// export const updateRecordStatusByDay = async (currentTime) => {
//     const response = await api.post("/record/updatestatus", {
//         Time: currentTime.date,
//     });
//     return response.data;
// };
import api from "../api";

export const fetchRecords = async () => {
    try {
        const response = await api.get("/record");
        return response.data;
    } catch (error) {
        console.error("fetchRecords error:", error);
        throw error;
    }
};

export const fetchRecordById = async (id) => {
    try {
        const response = await api.get(`/record/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchRecordById error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchRecordByBorrowingRequestId = async (id) => {
    try {
        const response = await api.get(`/record/by-request/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchRecordByBorrowingRequestId error for ID ${id}:`, error);
        throw error;
    }
};

export const deleteRecord = async (id) => {
    try {
        await api.delete(`/record/${id}`);
    } catch (error) {
        console.error(`deleteRecord error for ID ${id}:`, error);
        throw error;
    }
};

export const markBookPickedUp = async (id, adminId) => {
    try {
        // adminId should be an object with the structure { id: "adminIdValue" }
        const response = await api.put(`/record/pickup/${id}`, adminId);
        return response.data;
    } catch (error) {
        console.error(`markBookPickedUp error for ID ${id}:`, error);
        throw error;
    }
};

export const markBookReturned = async (id, adminId) => {
    try {
        // adminId should be an object with the structure { id: "adminIdValue" }
        const response = await api.put(`/record/return/${id}`, adminId);
        return response.data;
    } catch (error) {
        console.error(`markBookReturned error for ID ${id}:`, error);
        throw error;
    }
};

export const updateRecordStatusByDay = async (currentTime) => {
    try {
        // currentTime should be an object with the structure { date: "2025-05-06T15:06:09.793Z" }
        const response = await api.post("/record/updatestatus", currentTime);
        return response.data;
    } catch (error) {
        console.error(`updateRecordStatusByDay error:`, error);
        throw error;
    }
};

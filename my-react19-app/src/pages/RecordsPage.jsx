// export default RecordPage;
import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import {
    getRecordsUseCase,
    deleteRecordUseCase,
    markBookPickedUpUseCase,
    markBookReturnedUseCase,
    updateRecordStatusByDayUseCase,
} from "../domain/UseCases/recordUseCase";

const RecordPage = () => {
    const [records, setRecords] = useState([]);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [currentUser, setCurrentUser] = useState(null);

    // Fetch records and set up user
    useEffect(() => {
        const fetchData = async () => {
            try {
                const user = JSON.parse(localStorage.getItem("user")) || null;
                console.log("Current user:", user);
                setCurrentUser(user);
                const recordsData = await getRecordsUseCase();
                setRecords(recordsData);
                setTotalItems(recordsData.length);
            } catch (error) {
                setError(error.message || "Failed to fetch records");
                console.error("Failed to fetch records:", error);
            }
        };
        fetchData();
    }, []);
    // Set up 12-hour interval for updating record statuses
    useEffect(() => {
        const updateRecordsStatus = async () => {
            try {
                console.log("Updating record statuses at:", new Date().toLocaleString());

                // Format the date parameter properly according to API requirements
                const currentTime = { date: new Date().toISOString() };
                await updateRecordStatusByDayUseCase(currentTime);

                // Refresh records after updating statuses
                const recordsData = await getRecordsUseCase();
                setRecords(recordsData);
                setTotalItems(recordsData.length);

                console.log("Record statuses updated successfully");
                setError(null);
            } catch (error) {
                setError(error.message || "Failed to update record statuses");
                console.error("Failed to update record statuses:", error);
            }
        };

        // Run immediately on mount - this sets the starting point
        console.log("Initial status update on component mount at:", new Date().toLocaleString());
        updateRecordsStatus();

        // Set interval to run every 12 hours (43,200,000 ms)
        // This will start counting 12 hours from when the component is first mounted
        const intervalId = setInterval(updateRecordsStatus, 12 * 60 * 60 * 1000);
        console.log("Status update interval set for every 12 hours");

        // Cleanup interval on unmount
        return () => clearInterval(intervalId);
    }, []);
    const getDisplayStatus = (item) => {
        // If returnStatus is set, use it (Overdue or Not Picked Up)
        if (item.returnStatus !== null && item.returnStatus !== undefined) {
            return returnStatusMap[item.returnStatus] || "Unknown";
        }

        // Derive other statuses based on hasPickedUp and hasReturned
        if (item.hasReturned) {
            return "Returned"; // hasPickedUp = true, hasReturned = true
        }
        if (item.hasPickedUp) {
            return "Picked Up"; // hasPickedUp = true, hasReturned = false
        }
        return "Pending"; // hasPickedUp = false, hasReturned = false
    };
    const handleDelete = async (id) => {
        try {
            await deleteRecordUseCase(id);
            const updatedRecords = records.filter((r) => r.id !== id);
            setRecords(updatedRecords);
            setTotalItems(updatedRecords.length);
            const maxPage = Math.ceil(updatedRecords.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
            setError(null);
        } catch (error) {
            setError(error.message || "Failed to delete record");
            console.error("Failed to delete record:", error);
        }
    };

    const handleMarkPickedUp = async (id) => {
        try {
            if (!currentUser || !currentUser.id) {
                throw new Error("Admin ID is required. Please log in.");
            }

            // Create the payload object with the admin ID
            const adminId = { id: currentUser.id };

            await markBookPickedUpUseCase(id, adminId);

            // Refresh records after updating status
            const recordsData = await getRecordsUseCase();
            setRecords(recordsData);
            setTotalItems(recordsData.length);
            console.log("Mark Picked Up - ID:", id, "Admin ID:", currentUser.id);
            setError(null);
        } catch (error) {
            setError(error.message || "Failed to mark book picked up");
            console.error("Failed to mark book picked up:", error);
        }
    };

    const handleMarkReturned = async (id) => {
        try {
            if (!currentUser || !currentUser.id) {
                throw new Error("Admin ID is required. Please log in.");
            }

            // Create the payload object with the admin ID
            const adminId = { id: currentUser.id };

            await markBookReturnedUseCase(id, adminId);

            // Refresh records after updating status
            const recordsData = await getRecordsUseCase();
            setRecords(recordsData);
            setTotalItems(recordsData.length);
            console.log("Mark Returned - ID:", id, "Admin ID:", currentUser.id);
            setError(null);
        } catch (error) {
            setError(error.message || "Failed to mark book returned");
            console.error("Failed to mark book returned:", error);
        }
    };

    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return records.slice(startIndex, endIndex);
    };

    const statusMap = {
        0: "Overdue", // Matches API's ReturnStatus.Overdue
        1: "Not Picked Up", // Matches API's ReturnStatus.NotPickUp
    };

    const columns = [
        { key: "id", label: "ID" },
        {
            key: "borrowingRequestId",
            label: "Borrowing Request ID",
        },
        {
            key: "pickupDate",
            label: "Pickup Date",
            render: (item) => (item.pickUpDate ? new Date(item.pickUpDate).toLocaleString() : "—"),
        },
        // {
        //     key: "pickupAdminFullName",
        //     label: "Pickup Admin Name",
        //     render: (item) => (item.pickUpAdmin ? item.pickUpAdmin.fullName : "—"),
        // },
        // {
        //     key: "pickupAdminEmail",
        //     label: "Pickup Admin Email",
        //     render: (item) => (item.pickUpAdmin ? item.pickUpAdmin.email : "—"),
        // },
        {
            key: "returnDate",
            label: "Return Date",
            render: (item) => (item.returnDate ? new Date(item.returnDate).toLocaleString() : "—"),
        },
        // {
        //     key: "returnAdminFullName",
        //     label: "Return Admin Name",
        //     render: (item) => (item.returnAdmin ? item.returnAdmin.fullName : "—"),
        // },
        // {
        //     key: "returnAdminEmail",
        //     label: "Return Admin Email",
        //     render: (item) => (item.returnAdmin ? item.returnAdmin.email : "—"),
        // },
        {
            key: "status",
            label: "Status",
            render: (item) => getDisplayStatus(item),
        },
    ];

    const customActions = [
        {
            label: "Mark Picked Up",
            onClick: handleMarkPickedUp,
            condition: (item) => !item.hasPickedUp && item.returnStatus !== 1,
        },
        {
            label: "Mark Returned",
            onClick: handleMarkReturned,
            condition: (item) => item.hasPickedUp && !item.hasReturned, // Show for Picked Up or Overdue
        },
    ];

    return (
        <div>
            {error && <div className="mb-4 rounded border border-red-400 bg-red-100 px-4 py-3 text-red-700">{error}</div>}
            <CrudTable
                title="Borrowing Records"
                data={getPaginatedData()}
                columns={columns}
                formFields={[]}
                onAdd={null}
                onEdit={null}
                onDelete={handleDelete}
                customActions={customActions}
                pagination={{
                    current: currentPage,
                    pageSize: pageSize,
                    total: totalItems,
                    onChange: (page, pageSize) => {
                        setCurrentPage(page);
                        setPageSize(pageSize);
                    },
                    showSizeChanger: true,
                    pageSizeOptions: ["5", "10", "15", "20"],
                }}
            />
        </div>
    );
};

export default RecordPage;

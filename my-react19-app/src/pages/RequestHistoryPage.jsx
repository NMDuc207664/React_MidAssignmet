import React, { useState, useEffect } from "react";
import { Table, Button, Alert, DatePicker, Space, Popconfirm, Tooltip } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import moment from "moment";
import { getBorrowsByUserIdUseCase, deleteBorrowUseCase } from "../domain/UseCases/borrowUseCase";
import { accountRepository } from "../data/Repositories/accountRepository";
import { useNavigate } from "react-router-dom";

const RequestHistoryPage = () => {
    const navigate = useNavigate();
    const [requests, setRequests] = useState([]);
    const [filteredRequests, setFilteredRequests] = useState([]);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [loading, setLoading] = useState(false);
    const [monthFilter, setMonthFilter] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const user = accountRepository.getCurrentUser();
                if (!user) {
                    setError("You are not logged in. Redirecting to login...");
                    setTimeout(() => navigate("/login"), 2000);
                    return;
                }
                setCurrentUser(user);

                const userId = user.id;
                const data = await getBorrowsByUserIdUseCase(userId);
                console.log("RequestHistoryPage: Fetched requests:", data); // Debug
                setRequests(data);
                setFilteredRequests(data);
            } catch (error) {
                setError(error.message || "Failed to fetch request history.");
                console.error("RequestHistoryPage: Error fetching requests:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [navigate]);

    const statusMap = {
        0: { text: "Waiting", color: "gold" },
        1: { text: "Approved", color: "green" },
        2: { text: "Rejected", color: "red" },
    };

    const handleMonthFilterChange = (date) => {
        setMonthFilter(date);
        console.log("handleMonthFilterChange: Selected date:", date ? date.format("YYYY-MM") : "All");

        if (!date) {
            setFilteredRequests([...requests]);
            return;
        }

        // Chuyển đổi tháng và năm cho việc so sánh
        const selectedMonth = date.month();
        const selectedYear = date.year();

        const filtered = requests.filter((req) => {
            // Đảm bảo requestedDate là một đối tượng moment
            const requestDate = moment(req.requestedDate);

            // So sánh tháng và năm
            return requestDate.month() === selectedMonth && requestDate.year() === selectedYear;
        });

        console.log("handleMonthFilterChange: Filtered requests:", filtered);
        setFilteredRequests(filtered);
    };

    const handleDelete = async (id) => {
        setError(null);
        setSuccess(null);
        setLoading(true);
        try {
            await deleteBorrowUseCase(id);
            setSuccess("Request deleted successfully!");
            const userId = currentUser.id;
            const updatedRequests = await getBorrowsByUserIdUseCase(userId);
            setRequests(updatedRequests);

            // Áp dụng lại bộ lọc hiện tại nếu có
            if (monthFilter) {
                const selectedMonth = monthFilter.month();
                const selectedYear = monthFilter.year();

                const filtered = updatedRequests.filter((req) => {
                    const requestDate = moment(req.requestedDate);
                    return requestDate.month() === selectedMonth && requestDate.year() === selectedYear;
                });

                setFilteredRequests(filtered);
            } else {
                setFilteredRequests(updatedRequests);
            }
        } catch (error) {
            setError(error.message || "Failed to delete request.");
            console.error("RequestHistoryPage: Error deleting request:", error);
        } finally {
            setLoading(false);
        }
    };

    const canDeleteRequest = (requestedDate) => {
        if (currentUser?.role === "Admin") return true;
        const requestTime = moment(requestedDate);
        const now = moment();
        const minutesDiff = now.diff(requestTime, "minutes");
        return minutesDiff <= 10;
    };

    const columns = [
        {
            title: "ID",
            dataIndex: "id",
            key: "id",
            width: 100,
            ellipsis: true,
        },
        {
            title: "Requested Date",
            dataIndex: "requestedDate",
            key: "requestedDate",
            render: (date) => moment(date).format("YYYY-MM-DD HH:mm:ss"),
            sorter: (a, b) => moment(a.requestedDate).unix() - moment(b.requestedDate).unix(),
        },
        {
            title: "Status",
            dataIndex: "requestStatus",
            key: "requestStatus",
            render: (status) => <span style={{ color: statusMap[status]?.color }}>{statusMap[status]?.text || "Unknown"}</span>,
            filters: [
                { text: "Waiting", value: 0 },
                { text: "Approved", value: 1 },
                { text: "Rejected", value: 2 },
            ],
            onFilter: (value, record) => record.requestStatus === value,
        },
        {
            title: "Books",
            dataIndex: "borrowingDetails",
            key: "borrowingDetails",
            render: (details) => details?.map((bd) => bd.bookName).join(", ") || "—",
        },
        {
            title: "Full Name",
            dataIndex: "fullName",
            key: "fullName",
        },
        {
            title: "Email",
            dataIndex: "email",
            key: "email",
        },
        {
            title: "Phone Number",
            dataIndex: "phoneNumber",
            key: "phoneNumber",
        },
        {
            title: "Address",
            dataIndex: "address",
            key: "address",
        },
        {
            title: "Actions",
            key: "actions",
            render: (_, record) => (
                <Tooltip title={canDeleteRequest(record.requestedDate) ? "Delete this request" : "Cannot delete requests older than 10 minutes"}>
                    <Popconfirm
                        title="Are you sure you want to delete this request?"
                        onConfirm={() => handleDelete(record.id)}
                        okText="Yes"
                        cancelText="No"
                        disabled={!canDeleteRequest(record.requestedDate)}
                    >
                        <Button
                            icon={<DeleteOutlined />}
                            danger
                            disabled={!canDeleteRequest(record.requestedDate)}
                        >
                            Delete
                        </Button>
                    </Popconfirm>
                </Tooltip>
            ),
        },
    ];

    return (
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
            <div className="overflow-hidden rounded-lg bg-white shadow-lg">
                <div className="bg-gradient-to-r from-blue-500 to-blue-700 p-6 text-white">
                    <h1 className="text-3xl font-bold">Request History</h1>
                    <p className="mt-2">View and manage your borrowing request history</p>
                </div>

                <div className="p-6">
                    {error && (
                        <Alert
                            message={error}
                            type="error"
                            showIcon
                            className="mb-4"
                            closable
                            onClose={() => setError(null)}
                        />
                    )}
                    {success && (
                        <Alert
                            message={success}
                            type="success"
                            showIcon
                            className="mb-4"
                            closable
                            onClose={() => setSuccess(null)}
                        />
                    )}

                    <Space
                        direction="vertical"
                        size="middle"
                        style={{ width: "100%", marginBottom: 16 }}
                    >
                        <Space>
                            <span className="font-medium text-gray-700">Filter by Month:</span>
                            <DatePicker
                                picker="month"
                                value={monthFilter}
                                onChange={handleMonthFilterChange}
                                placeholder="Select month"
                                allowClear
                                format="YYYY-MM"
                            />
                        </Space>
                    </Space>

                    <Table
                        columns={columns}
                        dataSource={filteredRequests}
                        rowKey="id"
                        loading={loading}
                        pagination={{
                            current: 1,
                            pageSize: 10,
                            total: filteredRequests.length,
                            showSizeChanger: true,
                            pageSizeOptions: ["5", "10", "15", "20"],
                        }}
                        scroll={{ x: 1200 }}
                    />
                </div>
            </div>
        </div>
    );
};

export default RequestHistoryPage;

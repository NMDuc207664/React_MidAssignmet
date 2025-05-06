import React, { useState, useEffect } from "react";
import CrudTable from "../components/CrudTable";
import { getUsersUseCase, deleteUserUseCase, updateUserRoleUseCase, resetPasswordUseCase } from "../domain/UseCases/userUseCase";
const UsersPage = () => {
    const [users, setUsers] = useState([]);
    const [error, setError] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalItems, setTotalItems] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const usersData = await getUsersUseCase();
                setUsers(usersData);
                setTotalItems(usersData.length);
            } catch (error) {
                setError(error);
            }
        };

        fetchData();
    }, []);

    const columns = [
        { key: "id", label: "ID" },
        { key: "email", label: "Email" },
        { key: "role", label: "Role" },
    ];
    const formFields = [
        {
            key: "role",
            type: "select",
            label: "Role",
            required: true,
        },
    ];
    const selectOptions = {
        role: [
            { value: "User", label: "User" },
            { value: "Admin", label: "Admin" },
        ],
    };
    const handleDelete = async (id) => {
        // try {
        //     await deleteUserUseCase(id);
        //     const updatedUsers = users.filter((user) => user.id !== id);
        //     setUsers(updatedUsers);
        // } catch (error) {
        //     setError(error);
        // }
        try {
            await deleteUserUseCase(id);
            const updatedUsers = users.filter((user) => user.id !== id);
            setUsers(updatedUsers);
            setTotalItems(updatedUsers.length);

            const maxPage = Math.ceil(updatedUsers.length / pageSize);
            if (currentPage > maxPage && maxPage > 0) {
                setCurrentPage(maxPage);
            }
        } catch (error) {
            setError(error);
        }
    };
    const getPaginatedData = () => {
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return users.slice(startIndex, endIndex);
    };

    const handleResetPassword = async (id) => {
        try {
            await resetPasswordUseCase(id);
        } catch (error) {
            setError(error);
        }
    };

    const handleUpdateRole = async (id, role) => {
        try {
            // await updateUserRoleUseCase(id, { role });
            await updateUserRoleUseCase(id, { role: role.role });
            const updatedUsers = users.map((user) => {
                if (user.id === id) {
                    return { ...user, role: role.role };
                }
                return user;
            });
            setUsers(updatedUsers);
        } catch (error) {
            setError(error);
        }
    };

    return (
        <div>
            <h1>Users</h1>
            {error && <p>{error.message}</p>}
            <CrudTable
                title="Users"
                columns={columns}
                //data={users}
                data={getPaginatedData()}
                formFields={formFields}
                selectOptions={selectOptions}
                onDelete={handleDelete}
                onResetPassword={handleResetPassword}
                onEdit={handleUpdateRole}
                pagination={{
                    current: currentPage, // Trang hiện tại
                    pageSize: pageSize, // Số lượng item mỗi trang
                    total: totalItems, // Tổng số item
                    onChange: (page, pageSize) => {
                        setCurrentPage(page); // Cập nhật trang khi thay đổi
                        setPageSize(pageSize); // Cập nhật số lượng item mỗi trang
                    },
                    showSizeChanger: true, // Cho phép người dùng thay đổi số lượng item mỗi trang
                    pageSizeOptions: ["5", "10", "15", "20"], // Các lựa chọn số lượng item mỗi trang
                }}
            />
        </div>
    );
};
export default UsersPage;

import React, { useState } from "react";
import { Pagination, Button, Modal, Alert, Space, Popconfirm, Tooltip } from "antd";
import { EditOutlined, DeleteOutlined, LockOutlined, CheckCircleOutlined, UndoOutlined } from "@ant-design/icons";

const CrudTable = ({
    pagination,
    title = "Items",
    data,
    columns,
    onAdd,
    onEdit,
    onDelete,
    onResetPassword,
    formFields,
    selectOptions = {},
    customActions = [],
}) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [modalAction, setModalAction] = useState("create");
    const [formData, setFormData] = useState({});
    const [editingId, setEditingId] = useState(null);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    const openModal = (action, item = {}) => {
        setModalAction(action);
        setFormData(item);
        setEditingId(item.id || null);
        setIsModalOpen(true);
        setError(null);
        setSuccess(null);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        try {
            if (modalAction === "create") {
                onAdd?.(formData);
                setSuccess(`${title.slice(0, -1)} created successfully!`);
            } else {
                onEdit?.(editingId, formData);
                setSuccess(`${title.slice(0, -1)} updated successfully!`);
            }
            setIsModalOpen(false);
            setFormData({});
        } catch (err) {
            setError(err.message || "Failed to submit form.");
        }
    };

    const handleCancel = () => {
        setIsModalOpen(false);
        setFormData({});
        setError(null);
        setSuccess(null);
    };

    const handleDelete = (id) => {
        try {
            onDelete?.(id);
            setSuccess(`${title.slice(0, -1)} deleted successfully!`);
        } catch (err) {
            setError(err.message || "Failed to delete item.");
        }
    };

    const handleResetPassword = (id) => {
        try {
            onResetPassword?.(id);
            setSuccess("Password reset successfully!");
        } catch (err) {
            setError(err.message || "Failed to reset password.");
        }
    };

    // Map custom action labels to icons
    const actionIcons = {
        "Mark Picked Up": <CheckCircleOutlined />,
        "Mark Returned": <UndoOutlined />,
    };

    return (
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
            <div className="overflow-hidden rounded-lg bg-white shadow-lg">
                <div className="bg-gradient-to-r from-blue-500 to-blue-700 p-6 text-white">
                    <h1 className="text-3xl font-bold">{title}</h1>
                    <p className="mt-2">Manage your {title.toLowerCase()}</p>
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
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-bold text-gray-800">{title}</h2>
                        {onAdd && (
                            <Button
                                type="primary"
                                onClick={() => openModal("create")}
                            >
                                Add {title.slice(0, -1)}
                            </Button>
                        )}
                    </div>
                    <table className="w-full overflow-hidden rounded-lg bg-white shadow-md">
                        <thead className="bg-gray-100">
                            <tr>
                                {columns.map((col) => (
                                    <th
                                        key={col.key}
                                        className="px-4 py-2 text-left text-sm font-medium text-gray-700"
                                    >
                                        {col.label}
                                    </th>
                                ))}
                                <th className="px-4 py-2 text-left text-sm font-medium text-gray-700">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.map((item) => (
                                <tr
                                    key={item.id}
                                    className="border-t hover:bg-gray-50"
                                >
                                    {columns.map((col) => (
                                        <td
                                            key={col.key}
                                            className="px-4 py-2 text-sm text-gray-600"
                                        >
                                            {col.render ? col.render(item) : item[col.key]}
                                        </td>
                                    ))}
                                    <td className="px-6 py-4 text-sm whitespace-nowrap">
                                        <Space size="middle">
                                            {onEdit && (
                                                <Tooltip title={`Edit this ${title.slice(0, -1).toLowerCase()}`}>
                                                    <Button
                                                        icon={<EditOutlined />}
                                                        onClick={() => openModal("edit", item)}
                                                        type="primary"
                                                        shape="circle"
                                                    />
                                                </Tooltip>
                                            )}
                                            {onDelete && (
                                                <Tooltip title={`Delete this ${title.slice(0, -1).toLowerCase()}`}>
                                                    <Popconfirm
                                                        title={`Are you sure you want to delete this ${title.slice(0, -1).toLowerCase()}?`}
                                                        onConfirm={() => handleDelete(item.id)}
                                                        okText="Yes"
                                                        cancelText="No"
                                                    >
                                                        <Button
                                                            icon={<DeleteOutlined />}
                                                            danger
                                                            shape="circle"
                                                        />
                                                    </Popconfirm>
                                                </Tooltip>
                                            )}
                                            {onResetPassword && (
                                                <Tooltip title="Reset password">
                                                    <Popconfirm
                                                        title="Are you sure you want to reset this user's password?"
                                                        onConfirm={() => handleResetPassword(item.id)}
                                                        okText="Yes"
                                                        cancelText="No"
                                                    >
                                                        <Button
                                                            icon={<LockOutlined />}
                                                            shape="circle"
                                                        />
                                                    </Popconfirm>
                                                </Tooltip>
                                            )}
                                            {customActions.map(
                                                (action, index) =>
                                                    action.condition(item) && (
                                                        <Tooltip
                                                            key={index}
                                                            title={action.label}
                                                        >
                                                            <Popconfirm
                                                                title={`Are you sure you want to ${action.label.toLowerCase()}?`}
                                                                onConfirm={() => action.onClick(item.id)}
                                                                okText="Yes"
                                                                cancelText="No"
                                                            >
                                                                <Button
                                                                    icon={actionIcons[action.label] || <CheckCircleOutlined />}
                                                                    shape="circle"
                                                                    type="default"
                                                                />
                                                            </Popconfirm>
                                                        </Tooltip>
                                                    ),
                                            )}
                                        </Space>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    {pagination && (
                        <div className="mt-4 flex justify-end">
                            <Pagination
                                current={pagination.current}
                                pageSize={pagination.pageSize}
                                total={pagination.total}
                                onChange={pagination.onChange}
                                showSizeChanger={pagination.showSizeChanger}
                                pageSizeOptions={pagination.pageSizeOptions}
                            />
                        </div>
                    )}
                </div>
            </div>

            {isModalOpen && formFields && (
                <Modal
                    title={modalAction === "create" ? `Add ${title.slice(0, -1)}` : `Edit ${title.slice(0, -1)}`}
                    open={isModalOpen}
                    onOk={handleSubmit}
                    onCancel={handleCancel}
                    okText={modalAction === "create" ? "Create" : "Update"}
                    cancelText="Cancel"
                >
                    <form onSubmit={handleSubmit}>
                        {formFields
                            .filter((field) => !field.visible || field.visible(modalAction))
                            .map((field) => {
                                const isDisabled = typeof field.disabled === "function" ? field.disabled(modalAction) : field.disabled || false;
                                return (
                                    <div
                                        key={field.key}
                                        className="mb-4"
                                    >
                                        <label className="mb-1 block text-sm font-medium text-gray-700">{field.label}</label>
                                        {field.type === "select" ? (
                                            <select
                                                multiple={field.multiple}
                                                value={formData[field.key] || (field.multiple ? [] : "")}
                                                onChange={(e) => {
                                                    const value = field.multiple
                                                        ? Array.from(e.target.selectedOptions, (option) => option.value)
                                                        : e.target.value;
                                                    setFormData({ ...formData, [field.key]: value });
                                                }}
                                                className="w-full rounded border p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                                                disabled={isDisabled}
                                            >
                                                {!field.multiple && <option value="">Select {field.label}</option>}
                                                {selectOptions[field.key]?.map((option) => (
                                                    <option
                                                        key={option.value}
                                                        value={option.value}
                                                    >
                                                        {option.label}
                                                    </option>
                                                ))}
                                            </select>
                                        ) : (
                                            <input
                                                type={field.type || "text"}
                                                value={formData[field.key] || ""}
                                                onChange={(e) => setFormData({ ...formData, [field.key]: e.target.value })}
                                                className="w-full rounded border p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                                                disabled={isDisabled}
                                                placeholder={field.placeholder || `Enter ${field.label}`}
                                            />
                                        )}
                                    </div>
                                );
                            })}
                    </form>
                </Modal>
            )}
        </div>
    );
};

export default CrudTable;

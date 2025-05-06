import React, { useState, useEffect } from "react";
import { accountRepository } from "../data/Repositories/accountRepository";
import { getUserByIdUseCase, updateUserProfileUseCase, changePasswordUseCase } from "../domain/UseCases/userUseCase";
import { UserOutlined, KeyOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

export default function ProfilePage() {
    const navigate = useNavigate();
    const [user, setUser] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [profileData, setProfileData] = useState({});
    const [passwordData, setPasswordData] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    });
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchUser = async () => {
            setIsLoading(true);
            const currentUser = accountRepository.getCurrentUser();
            const token = localStorage.getItem("auth_token");
            console.log("ProfilePage: Current User:", currentUser); // Debug
            console.log("ProfilePage: Auth Token:", token); // Debug

            if (!accountRepository.isLoggedIn() || !currentUser?.id) {
                console.error("ProfilePage: No valid user session or user ID, redirecting to login");
                setError("You are not logged in or session is invalid. Redirecting to login...");
                setTimeout(() => navigate("/login"), 2000);
                setIsLoading(false);
                return;
            }

            try {
                const userData = await getUserByIdUseCase(currentUser.id);
                console.log("ProfilePage: Fetched User Data:", userData); // Debug
                setUser(userData);
                setProfileData({
                    firstName: userData.firstName || "",
                    lastName: userData.lastName || "",
                    address: userData.address || "",
                    phoneNumber: userData.phoneNumber || "",
                    email: userData.email || "",
                    username: userData.userName || userData.username || "",
                });
                // Update localStorage with complete user data
                localStorage.setItem(
                    "user",
                    JSON.stringify({
                        ...currentUser,
                        ...userData,
                        id: currentUser.id, // Preserve original ID
                    }),
                );
            } catch (err) {
                console.error("ProfilePage: Error fetching user:", err);
                if (err.message.includes("Unauthorized") || err.message.includes("not authenticated")) {
                    setError("Session expired or invalid. Please log in again.");
                    setTimeout(() => navigate("/login"), 2000);
                } else {
                    setError("Failed to load user profile. Please try again.");
                }
            }
            setIsLoading(false);
        };
        fetchUser();
    }, [navigate]);

    const handleEditToggle = () => {
        setIsEditing(!isEditing);
        setError("");
        setSuccess("");
    };

    const handleProfileChange = (e) => {
        setProfileData({ ...profileData, [e.target.name]: e.target.value });
    };

    const handlePasswordChange = (e) => {
        setPasswordData({ ...passwordData, [e.target.name]: e.target.value });
    };

    const handleProfileSubmit = async (e) => {
        e.preventDefault();
        try {
            await updateUserProfileUseCase(user.id, profileData);
            const updatedUser = await getUserByIdUseCase(user.id);
            setUser(updatedUser);
            localStorage.setItem(
                "user",
                JSON.stringify({
                    ...accountRepository.getCurrentUser(),
                    ...updatedUser,
                }),
            );
            setSuccess("Profile updated successfully!");
            setIsEditing(false);
        } catch (err) {
            setError("Failed to update profile.");
            console.error("ProfilePage: Error updating profile:", err);
        }
    };

    const handlePasswordSubmit = async (e) => {
        e.preventDefault();
        if (passwordData.newPassword !== passwordData.confirmPassword) {
            setError("New password and confirm password do not match.");
            return;
        }
        try {
            await changePasswordUseCase(user.id, passwordData);
            setSuccess("Password changed successfully!");
            setPasswordData({ currentPassword: "", newPassword: "", confirmPassword: "" });
        } catch (err) {
            setError("Failed to change password.");
            console.error("ProfilePage: Error changing password:", err);
        }
    };

    if (isLoading) {
        return (
            <div className="flex h-full items-center justify-center">
                <div className="text-gray-600">Loading profile...</div>
            </div>
        );
    }

    if (!user && error) {
        return (
            <div className="flex h-full items-center justify-center">
                <div className="text-red-600">{error}</div>
            </div>
        );
    }

    return (
        <div className="mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:px-8">
            <div className="overflow-hidden rounded-lg bg-white shadow-lg">
                <div className="bg-gradient-to-r from-blue-500 to-blue-700 p-6 text-white">
                    <h1 className="text-3xl font-bold">User Profile</h1>
                    <p className="mt-2">Manage your personal information and account settings</p>
                </div>

                <div className="p-6">
                    {error && <div className="mb-4 rounded-md bg-red-100 p-3 text-red-700">{error}</div>}
                    {success && <div className="mb-4 rounded-md bg-green-100 p-3 text-green-700">{success}</div>}

                    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                        <div>
                            <div className="mb-4 flex items-center">
                                <UserOutlined
                                    className="mr-2 text-blue-600"
                                    style={{ fontSize: 20 }}
                                />
                                <h2 className="text-xl font-semibold">Personal Information</h2>
                            </div>
                            {isEditing ? (
                                <form onSubmit={handleProfileSubmit}>
                                    <div className="space-y-4">
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700">First Name</label>
                                            <input
                                                type="text"
                                                name="firstName"
                                                value={profileData.firstName}
                                                onChange={handleProfileChange}
                                                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                            />
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700">Last Name</label>
                                            <input
                                                type="text"
                                                name="lastName"
                                                value={profileData.lastName}
                                                onChange={handleProfileChange}
                                                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                            />
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700">Address</label>
                                            <input
                                                type="text"
                                                name="address"
                                                value={profileData.address}
                                                onChange={handleProfileChange}
                                                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                            />
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700">Phone Number</label>
                                            <input
                                                type="text"
                                                name="phoneNumber"
                                                value={profileData.phoneNumber}
                                                onChange={handleProfileChange}
                                                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                            />
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700">Email</label>
                                            <input
                                                type="email"
                                                name="email"
                                                value={profileData.email}
                                                onChange={handleProfileChange}
                                                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                            />
                                        </div>
                                    </div>
                                    <div className="mt-6 flex gap-4">
                                        <button
                                            type="submit"
                                            className="rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                                        >
                                            Save Changes
                                        </button>
                                        <button
                                            type="button"
                                            onClick={handleEditToggle}
                                            className="rounded-md bg-gray-200 px-4 py-2 text-gray-700 hover:bg-gray-300 focus:outline-none"
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            ) : (
                                <div className="space-y-4">
                                    <div>
                                        <p className="text-sm text-gray-500">Full Name</p>
                                        <p className="text-lg font-medium">{`${user.firstName || ""} ${user.lastName || ""}`.trim()}</p>
                                    </div>
                                    <div>
                                        <p className="text-sm text-gray-500">Username</p>
                                        <p className="text-lg font-medium">{user.userName || user.username}</p>
                                    </div>
                                    <div>
                                        <p className="text-sm text-gray-500">Email</p>
                                        <p className="text-lg font-medium">{user.email}</p>
                                    </div>
                                    <div>
                                        <p className="text-sm text-gray-500">Phone Number</p>
                                        <p className="text-lg font-medium">{user.phoneNumber}</p>
                                    </div>
                                    <div>
                                        <p className="text-sm text-gray-500">Address</p>
                                        <p className="text-lg font-medium">{user.address}</p>
                                    </div>
                                    <div>
                                        <p className="text-sm text-gray-500">Role</p>
                                        <p className="text-lg font-medium">{user.role}</p>
                                    </div>
                                    <button
                                        onClick={handleEditToggle}
                                        className="mt-4 rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                                    >
                                        Edit Profile
                                    </button>
                                </div>
                            )}
                        </div>

                        <div>
                            <div className="mb-4 flex items-center">
                                <KeyOutlined
                                    className="mr-2 text-blue-600"
                                    style={{ fontSize: 20 }}
                                />
                                <h2 className="text-xl font-semibold">Change Password</h2>
                            </div>
                            <form onSubmit={handlePasswordSubmit}>
                                <div className="space-y-4">
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700">Current Password</label>
                                        <input
                                            type="password"
                                            name="currentPassword"
                                            value={passwordData.currentPassword}
                                            onChange={handlePasswordChange}
                                            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700">New Password</label>
                                        <input
                                            type="password"
                                            name="newPassword"
                                            value={passwordData.newPassword}
                                            onChange={handlePasswordChange}
                                            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700">Confirm New Password</label>
                                        <input
                                            type="password"
                                            name="confirmPassword"
                                            value={passwordData.confirmPassword}
                                            onChange={handlePasswordChange}
                                            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring-blue-500 focus:outline-none"
                                        />
                                    </div>
                                </div>
                                <button
                                    type="submit"
                                    className="mt-6 rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                                >
                                    Change Password
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { registerUseCase } from "../domain/UseCases/registerUseCase";

function RegisterPage() {
  const [formData, setFormData] = useState({
    firstname: "",
    lastname: "",
    email: "",
    username: "",
    password: "",
    comparePassword: "",
    phoneNumber: "",
    address: "",
  });
  const [error, setError] = useState("");
  const [errors, setErrors] = useState([]); // New state for array of errors
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (
      !formData.firstname ||
      !formData.lastname ||
      !formData.email ||
      !formData.username ||
      !formData.password ||
      !formData.comparePassword ||
      !formData.phoneNumber ||
      !formData.address
    ) {
      setError("All fields are required");
      setErrors([]);
      return;
    }
    if (formData.password !== formData.comparePassword) {
      setError("Passwords do not match");
      setErrors([]);
      return;
    }
    if (formData.password.length < 8) {
      setError("Password must be at least 8 characters");
      setErrors([]);
      return;
    }
    if (!formData.email.includes("@")) {
      setError("Invalid email");
      setErrors([]);
      return;
    }
    setLoading(true);
    setError("");
    setErrors([]);

    try {
      // Map front-end field names to match API model
      const apiData = {
        firstName: formData.firstname,
        lastName: formData.lastname,
        email: formData.email,
        userName: formData.username,
        password: formData.password,
        comparePassword: formData.comparePassword,
        phoneNumber: formData.phoneNumber,
        address: formData.address,
      };

      const result = await registerUseCase(apiData);

      // Check if registration was successful
      if (
        result &&
        result.isSuccessfulRegistration === false &&
        result.errors
      ) {
        // Handle specific API errors
        setErrors(result.errors);
      } else {
        navigate("/login");
      }
    } catch (err) {
      // Handle error response from API
      if (err && err.errors) {
        setErrors(err.errors);
      } else if (err && err.errorMessage) {
        setError(err.errorMessage);
      } else {
        setError(err.message || "Registration failed");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Register</h2>

        {/* Display single error message */}
        {error && <p className="text-red-500 mb-4">{error}</p>}

        {/* Display array of errors from API */}
        {errors.length > 0 && (
          <div className="bg-red-50 border border-red-200 rounded-md p-3 mb-4">
            <ul className="list-disc pl-5">
              {errors.map((err, index) => (
                <li key={index} className="text-red-600 text-sm">
                  {err}
                </li>
              ))}
            </ul>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="firstname">
              First Name
            </label>
            <input
              type="text"
              id="firstname"
              name="firstname"
              value={formData.firstname}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter first name"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="lastname">
              Last Name
            </label>
            <input
              type="text"
              id="lastname"
              name="lastname"
              value={formData.lastname}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter last name"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="username">
              Username
            </label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.username}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter username"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="email">
              Email
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter email"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="phoneNumber">
              Phone Number
            </label>
            <input
              type="text"
              id="phoneNumber"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter phone number"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="address">
              Address
            </label>
            <input
              type="text"
              id="address"
              name="address"
              value={formData.address}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter address"
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 mb-2" htmlFor="password">
              Password
            </label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter password"
            />
          </div>
          <div className="mb-6">
            <label
              className="block text-gray-700 mb-2"
              htmlFor="comparePassword"
            >
              Confirm Password
            </label>
            <input
              type="password"
              id="comparePassword"
              name="comparePassword"
              value={formData.comparePassword}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Confirm password"
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-white text-blue-500 py-2 rounded-lg border border-blue-500 hover:opacity-80 transition-opacity disabled:bg-gray-100 disabled:text-gray-400 disabled:border-gray-300"
          >
            {loading ? "Registering..." : "Register"}
          </button>
        </form>
        <p className="mt-4 text-center">
          Already have an account?{" "}
          <Link to="/login" className="text-blue-500 hover:underline">
            Login
          </Link>
        </p>
      </div>
    </div>
  );
}

export default RegisterPage;

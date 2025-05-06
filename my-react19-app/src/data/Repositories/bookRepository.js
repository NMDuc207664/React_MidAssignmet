import api from "../api";

export const fetchBooks = async () => {
    try {
        const response = await api.get("/book");
        return response.data;
    } catch (error) {
        console.error("fetchBooks error:", error);
        throw error;
    }
};

export const fetchBookById = async (id) => {
    try {
        const response = await api.get(`/book/${id}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBookById error for ID ${id}:`, error);
        throw error;
    }
};

export const fetchBooksByAuthorId = async (authorId) => {
    try {
        const response = await api.get(`/book/by-author/${authorId}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBooksByAuthorId error for Author ID ${authorId}:`, error);
        throw error;
    }
};

export const fetchBooksByGenreId = async (genreId) => {
    try {
        const response = await api.get(`/api/book/by-genre/${genreId}`);
        return response.data;
    } catch (error) {
        console.error(`fetchBooksByGenreId error for Genre ID ${genreId}:`, error);
        throw error;
    }
};

export const createBook = async (bookData) => {
    try {
        const response = await api.post("/book", {
            Name: bookData.name,
            Description: bookData.description,
            StorageNumber: bookData.storageNumber,
            BookAuthors: bookData.bookAuthors, // Array of { AuthorID }
            BookGenres: bookData.bookGenres, // Array of { GenreId }
        });
        return response.data;
    } catch (error) {
        console.error("createBook error:", error);
        throw error;
    }
};

export const updateBook = async (id, bookData) => {
    try {
        const response = await api.put(`/book/${id}`, {
            Name: bookData.name,
            Description: bookData.description,
            StorageNumber: bookData.storageNumber,
            BookAuthors: bookData.bookAuthors, // Array of { AuthorID }
            BookGenres: bookData.bookGenres, // Array of { GenreId }
        });
        return response.data;
    } catch (error) {
        console.error(`updateBook error for ID ${id}:`, error);
        throw error;
    }
};

export const deleteBook = async (id) => {
    try {
        await api.delete(`/book/${id}`);
    } catch (error) {
        console.error(`deleteBook error for ID ${id}:`, error);
        throw error;
    }
};

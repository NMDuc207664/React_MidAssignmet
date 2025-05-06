import { fetchBooks, fetchBookById, createBook, updateBook, deleteBook } from "../../data/Repositories/bookRepository";

export const getBooksUseCase = async () => {
    try {
        const books = await fetchBooks();
        return books;
    } catch (error) {
        console.error("getBooksUseCase error:", error);
        throw error;
    }
};

export const getBookByIdUseCase = async (id) => {
    try {
        const book = await fetchBookById(id);
        return book;
    } catch (error) {
        console.error(`getBookByIdUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const createBookUseCase = async (bookData) => {
    try {
        const newBook = await createBook(bookData);
        return newBook;
    } catch (error) {
        console.error("createBookUseCase error:", error);
        throw error;
    }
};

export const updateBookUseCase = async (id, bookData) => {
    try {
        const updatedBook = await updateBook(id, bookData);
        return updatedBook;
    } catch (error) {
        console.error(`updateBookUseCase error for ID ${id}:`, error);
        throw error;
    }
};

export const deleteBookUseCase = async (id) => {
    try {
        await deleteBook(id);
    } catch (error) {
        console.error(`deleteBookUseCase error for ID ${id}:`, error);
        throw error;
    }
};

import { fetchAuthors, fetchAuthorById, createAuthor, updateAuthor, deleteAuthor } from "../../data/Repositories/authorRepository";

export const getAuthorsUseCase = async () => {
    return await fetchAuthors();
};

export const getAuthorByIdUseCase = async (id) => {
    return await fetchAuthorById(id);
};

export const createAuthorUseCase = async (authorData) => {
    return await createAuthor(authorData);
};

export const updateAuthorUseCase = async (id, authorData) => {
    return await updateAuthor(id, authorData);
};

export const deleteAuthorUseCase = async (id) => {
    return await deleteAuthor(id);
};

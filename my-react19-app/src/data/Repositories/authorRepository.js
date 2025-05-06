import api from "../api";

export const fetchAuthors = async () => {
    const response = await api.get("/author");
    return response.data;
};

export const fetchAuthorById = async (id) => {
    const response = await api.get(`/author/${id}`);
    return response.data;
};

export const createAuthor = async (authorData) => {
    const response = await api.post("/author", authorData);
    return response.data;
};

export const updateAuthor = async (id, authorData) => {
    const response = await api.put(`/author/${id}`, authorData);
    return response.data;
};

export const deleteAuthor = async (id) => {
    await api.delete(`/author/${id}`);
};

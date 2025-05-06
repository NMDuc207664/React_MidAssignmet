import api from "../api";

export const fetchGenres = async () => {
    const response = await api.get("/genre");
    return response.data;
};

export const fetchGenreById = async (id) => {
    const response = await api.get(`/genre/${id}`);
    return response.data;
};

export const createGenre = async (genreData) => {
    const response = await api.post("/genre", genreData);
    return response.data;
};
export const updateGenre = async (id, authorData) => {
    const response = await api.put(`/genre/${id}`, authorData);
    return response.data;
};

export const deleteGenre = async (id) => {
    await api.delete(`/genre/${id}`);
};

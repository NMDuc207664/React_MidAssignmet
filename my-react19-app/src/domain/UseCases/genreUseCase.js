import { fetchGenres, fetchGenreById, createGenre, updateGenre, deleteGenre } from "../../data/Repositories/genreRepository";
export const getGenresUseCase = async () => {
    return await fetchGenres();
};
export const getGenreByIdUseCase = async (id) => {
    return await fetchGenreById(id);
};
export const createGenreUseCase = async (genreData) => {
    return await createGenre(genreData);
};
export const updateGenreUseCase = async (id, genreData) => {
    return await updateGenre(id, genreData);
};
export const deleteGenreUseCase = async (id) => {
    return await deleteGenre(id);
};

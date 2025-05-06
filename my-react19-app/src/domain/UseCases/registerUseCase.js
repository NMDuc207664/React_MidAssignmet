import { accountRepository } from "../../data/Repositories/accountRepository";
export const registerUseCase = async (userData) => {
  return await accountRepository.register(userData);
};

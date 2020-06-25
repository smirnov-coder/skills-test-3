// Хук для работы с localStorage.
export function useLocalStorage(key: string): [() => string | null, (value: string) => void, () => void] {
    // Сохраняет значение в localStorage.
    const setValue = (value: string) => {
        localStorage.setItem(key, value);
    }

    // Извлекает значение из localStorage.
    const getValue = () => {
        return localStorage.getItem(key);
    }

    // Удаляет значение в localStorage.
    const clearValue = () => {
        localStorage.removeItem(key);
    }

    return [getValue, setValue, clearValue];
}

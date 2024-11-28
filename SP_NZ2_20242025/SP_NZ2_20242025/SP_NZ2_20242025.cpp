#include <iostream>
#include <cstdlib>
#include <ctime>
#include <Windows.h>


extern "C" __declspec(dllimport) void MergeSort(int* arr, int n);
extern "C" __declspec(dllimport) void HeapSort(int* arr, int n);
extern "C" __declspec(dllimport) void BubbleSort(int* arr, int n);

void generateRandomArray(int* arr, int size) {
    for (int i = 0; i < size; i++) {
        arr[i] = rand() % RAND_MAX;
    }
}

void printArray(int* arr, int size) {
    for (int i = 0; i < size; i++) {
        std::cout << arr[i] << " ";
    }
    std::cout << std::endl;
}

int main() {
    srand(static_cast<unsigned>(time(0)));

    
    const int size = 100000;
    int arr[size];
    generateRandomArray(arr, size);

    
    int choice;
    std::cout << "Select sorting algorithm: \n";
    std::cout << "1. Merge Sort\n";
    std::cout << "2. Heap Sort\n";
    std::cout << "3. Bubble Sort\n";
    std::cin >> choice;

    
    HMODULE hModule = NULL;

    if (choice == 1) {
        hModule = LoadLibrary(L"MergeSortDLL.dll");
    }
    else if (choice == 2) {
        hModule = LoadLibrary(L"HeapSortDLL.dll");
    }
    else if (choice == 3) {
        hModule = LoadLibrary(L"BubbleSortDLL.dll");
    }
    else {
        std::cout << "Invalid choice. Exiting...\n";
        return 1;
    }

    if (hModule == NULL) {
        std::cerr << "Error loading DLL\n";
        return 1;
    }

    typedef void (*SortFunc)(int*, int);
    SortFunc Sort = NULL;

    if (choice == 1) {
        Sort = (SortFunc)GetProcAddress(hModule, "MergeSort");
    }
    else if (choice == 2) {
        Sort = (SortFunc)GetProcAddress(hModule, "HeapSort");
    }
    else if (choice == 3) {
        Sort = (SortFunc)GetProcAddress(hModule, "BubbleSort");
    }

    if (Sort == NULL) {
        std::cerr << "Error finding function in DLL\n";
        FreeLibrary(hModule);
        return 1;
    }

    clock_t start = clock();
    Sort(arr, size);
    clock_t end = clock();

    double timeTaken = double(end - start) / CLOCKS_PER_SEC;
    std::cout << "Sorting took " << timeTaken << " seconds.\n";

    FreeLibrary(hModule);

    return 0;
}

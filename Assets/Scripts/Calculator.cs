using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 Пример 6 столбцов 4 строки: 
[] [] [] [] [] [] [0] [] [] [] []
[] [] [] [] [] [] [0] [] [] [] []
 */

/// <summary>
/// Класс выполняет все расчёты.
/// </summary>
public class Calculator : MonoBehaviour
{
    void Start(){}

    void Update(){ }

    private static Random randNumber = new Random();
    private static List<int[,]> list = new List<int[,]>();
    /// <summary>
    /// Метод генерирует массив рандомных неповторяющихся дробей.
    /// </summary>
    /// <returns>
    /// Двумерный массив [2, column] рандомных дробей. 
    /// Первые column дроби, следующая дробь 0/0 для разделения, после row дроби (см. пример перед классом)
    /// </returns>
    public int[,] CalculatingProbabilities(int row, int column, int gameLvl)
    {
        int sum = row + column + 1;
        int[,] mass = new int[2, (sum)], fraction = new int[2, 1];

        for (int j = 0; j < sum; j++)
        {
            if (j == column)
            {
                for (int i = 0; i < 2; i++)
                    mass[i, j] = 0;
                continue;
            }
            fraction = RandFraction(gameLvl);

            if (!checkReiteratAndReduct(fraction[0, 0], fraction[1, 0]))
            {
                j--;
                continue;
            }

            for (int i = 0; i < 2; i++)
                mass[i, j] = fraction[i, 0];
        }

        list.Clear();
        return mass;
    }

    /// <summary>
    /// Метод генерирует рандомное число (диапазоны можно будет изменить)
    /// </summary>
    /// <returns>
    /// Дробь в виде массива [2, 1]
    /// </returns>
    private static int[,] RandFraction(int gameLvl)
    {
        int denominator = Random.Range(2, 7 + gameLvl);
        int numerator = Random.Range(1, denominator);
        return new int[2, 1] { { numerator }, { denominator } };
    }

    /// <summary>
    /// Метод проверяет дробь на повторение и сокращение. На вход 1 арг - числитель, 2 арг - знаменатель.
    /// </summary>
    /// <returns>
    /// true - дробь не повторяется, false - дробь повторяется
    /// </returns>
    private static bool checkReiteratAndReduct(int num, int den)
    {
        for (int i = 2; i <= 5; i++)
            if (num % i == 0 && den % i == 0)
                return false;

        foreach (int[,] a in list)
            if (a[0, 0] == num && a[1, 0] == den)
                return false;
        list.Add(new int[2, 1] { { num }, { den } });
        return true;
    }

    /// <summary>
    /// Метод перемножает две дроби.
    /// На вход: 1 арг - числитель 1, 2 арг - числитель 2, 3 и 4 знаменатели 1 и 2 дробей
    /// </summary>
    /// <returns>
    /// Дробь в виде массива [2, 1]
    /// </returns>
    public static int[,] multiplyProbabilities(int numOne, int numTwo, int denOne, int denTwo)
    {
        int numerator = numOne * numTwo;
        int denominator = denOne * denTwo;
        return new int[2, 1] { { numerator }, { denominator } };
    }

    /// <summary>
    /// Метод сравнивает две дроби. На вход два массива [2, 1]
    /// </summary>
    /// <returns>
    /// Номер аргумента большей дроби. 0 если равны.
    /// </returns>
    public static int compareProbabilities(int[,] one, int[,] two)
    {
        int countOne = one[0, 0] * two[1, 0];
        int countTwo = two[0, 0] * one[1, 0];
        if (countOne > countTwo) return 1;
        else if (countOne < countTwo) return 2;
        else return 0;
    }

    /// <summary>
    /// Метод разворачивает массив поля в двумерный массив. 
    /// Вход: количество строк, 2 - столбцов, 3 - трёхмерный массив [row, column, 2], который является полем 
    /// </summary>
    /// <returns>
    /// Массив развёрнутого игрового поля [2, row*column], заполненый вероятностями
    /// </returns>
    private static int[,] RevMass(int str, int col, int[,,] arr)
    {
        int x = str - 1, y = col - 1, e = x, f = y, iter = 0, gran = 0, prov = 0, j = 0, povX, povY, sum = str * col;
        int[,] finalArray = new int[2, str * col];
        int[,] sled = new int[4, 5] {{ 0,-1, 0, 0, 1},
                                     {-1, 0, 0, f, 1},
                                     { 0, 1, e, f,-1},
                                     { 1, 0, e, 0,-1}};
        finalArray[0, j] = arr[x, y, 0];
        finalArray[1, j] = arr[x, y, 1];
        j++;
        while (j < sum)
        {
            povX = sled[iter, 2] + gran * sled[iter, 4];
            if (povX < 0) povX = -povX;
            povY = sled[iter, 3] + gran * sled[iter, 4];
            if (povY < 0)
                povY = -povY;
            while ((x != povX) && (y != povY))
            {
                x += sled[iter, 0];
                y += sled[iter, 1];
                finalArray[0, j] = arr[x, y, 0];
                finalArray[1, j] = arr[x, y, 1];
                j++;
                prov = 1;
            }

            if (prov == 0)
            {
                x += sled[iter, 0];
                y += sled[iter, 1];
                finalArray[0, j] = arr[x, y, 0];
                finalArray[1, j] = arr[x, y, 1];
                j++;
                prov = 0;
            }
            prov = 0;
            if (iter == 3) iter = 0;
            else if (iter == 2)
            {
                gran++;
                iter++;
            }
            else iter++;
        }
        return finalArray;
    }

    /// <summary>
    /// Метод формирует трёхмерный массив [row, column, 2] (поле с расчианными вероятностями)
    /// 3 аргумент на вход - выход с метода CalculatingProbabilities
    /// </summary>
    /// <returns>
    /// Готовый развёрнутый массив с вероятностями для всего поля [2, row * column]
    /// </returns>
    public int[,] FormationField(int[,] probabilitiesArray, int row, int column)
    {
        int[,] rowMass = new int[2, row], columnMass = new int[2, column], drob = new int[2, 1];
        bool flag = true;
        int count = 0;
        for (int i = 0; i < row + column + 1; i++)
        {
            if (flag)
            {
                if (probabilitiesArray[0, i] == 0)
                {
                    flag = false;
                    count = 0;
                    continue;
                }
                columnMass[0, i] = probabilitiesArray[0, i];
                columnMass[1, i] = probabilitiesArray[1, i];
                count++;
            }
            else
            {
                rowMass[0, count] = probabilitiesArray[0, i];
                rowMass[1, count] = probabilitiesArray[1, i];
                count++;
            }
        }
        int[,,] mass3D = new int[row, column, 2];
        for (int i = 0; i < row; i++)
            for (int j = 0; j < column; j++)
            {
                drob = multiplyProbabilities(columnMass[0, j], rowMass[0, i], columnMass[1, j], rowMass[1, i]);
                mass3D[i, j, 0] = drob[0, 0];
                mass3D[i, j, 1] = drob[1, 0];
            }
        int[,] final = new int[2, row * column];
        final = RevMass(row, column, mass3D);
        return final;
    }

}

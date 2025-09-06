using System;
using System.Globalization;

namespace SimpleCalculator
{
    class Program
    {
        static double memory = 0; // Переменная для памяти калькулятора

        static void Main(string[] args)
        {
            Console.WriteLine("=== Простой калькулятор ===");
            Console.WriteLine("Поддерживаемые операции:");
            Console.WriteLine("Бинарные: +, -, *, /, % (остаток от деления)");
            Console.WriteLine("Унарные: 1/x (обратное число), x^2 (квадрат), sqrt (корень квадратный)");
            Console.WriteLine("Функции памяти: M+, M-, MR");
            Console.WriteLine("Для выхода введите 'q' или 'quit'\n");

            while (true)
            {
                try
                {
                    Console.Write("Введите первое число или команду памяти (M+, M-, MR): ");
                    string input1 = Console.ReadLine();
                    if (IsExitCommand(input1))
                        break;

                    // Проверяем команды памяти
                    if (IsMemoryCommand(input1))
                    {
                        HandleMemoryCommand(input1);
                        continue;
                    }

                    if (!double.TryParse(input1, NumberStyles.Float, CultureInfo.InvariantCulture, out double num1))
                    {
                        Console.WriteLine("Ошибка: Некорректный формат первого числа\n");
                        continue;
                    }

                    Console.Write("Введите операцию (+, -, *, /, %, 1/x, x^2, sqrt): ");
                    string operation = Console.ReadLine()?.Trim();
                    if (IsExitCommand(operation))
                        break;

                    if (string.IsNullOrEmpty(operation) || !IsValidOperation(operation))
                    {
                        Console.WriteLine("Ошибка: Неподдерживаемая операция\n");
                        continue;
                    }

                    if (IsUnaryOperation(operation))
                    {
                        double result = PerformUnaryCalculation(num1, operation);
                        Console.WriteLine($"Результат: {operation}({num1}) = {result:G15}");
                    }
                    else
                    {
                        Console.Write("Введите второе число: ");
                        string input2 = Console.ReadLine();
                        if (IsExitCommand(input2))
                            break;

                        if (!double.TryParse(input2, NumberStyles.Float, CultureInfo.InvariantCulture, out double num2))
                        {
                            Console.WriteLine("Ошибка: Некорректный формат второго числа\n");
                            continue;
                        }

                        double result = PerformBinaryCalculation(num1, num2, operation);
                        Console.WriteLine($"Результат: {num1} {operation} {num2} = {result:G15}");
                    }

                    Console.WriteLine(new string('-', 40));
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Ошибка: Деление на ноль недопустимо\n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка операции: {ex.Message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}\n");
                }
            }

            Console.WriteLine("До свидания!");
        }

        static bool IsExitCommand(string input)
        {
            return string.Equals(input?.Trim(), "q", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input?.Trim(), "quit", StringComparison.OrdinalIgnoreCase);
        }

        static bool IsValidOperation(string operation)
        {
            return operation == "+" || operation == "-" || operation == "*" || operation == "/" ||
            operation == "%" || operation == "1/x" || operation == "x^2" || operation == "sqrt";
        }

        static bool IsUnaryOperation(string operation)
        {
            return operation == "1/x" || operation == "x^2" || operation == "sqrt";
        }

        static double PerformBinaryCalculation(double num1, double num2, string operation)
        {
            return operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" when num2 != 0 => num1 / num2,
                "/" when num2 == 0 => throw new DivideByZeroException(),
                "%" when num2 != 0 => num1 % num2,
                "%" when num2 == 0 => throw new DivideByZeroException(),
                _ => throw new ArgumentException("Неподдерживаемая бинарная операция")
            };
        }

        static double PerformUnaryCalculation(double num, string operation)
        {
            return operation switch
            {
                "1/x" when num != 0 => 1.0 / num,
                "1/x" when num == 0 => throw new DivideByZeroException(),
                "x^2" => num * num,
                "sqrt" when num >= 0 => Math.Sqrt(num),
                "sqrt" when num < 0 => throw new InvalidOperationException("Корень из отрицательного числа невозможен"),
                _ => throw new ArgumentException("Неподдерживаемая унарная операция")
            };
        }

        // Новые методы для работы с памятью
        static bool IsMemoryCommand(string input)
        {
            return string.Equals(input?.Trim(), "M+", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input?.Trim(), "M-", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input?.Trim(), "MR", StringComparison.OrdinalIgnoreCase);
        }

        static void HandleMemoryCommand(string command)
        {
            switch (command.ToUpper())
            {
                case "M+":
                    Console.Write("Введите число для добавления в память: ");
                    if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double addVal))
                    {
                        memory += addVal;
                        Console.WriteLine($"Память: {memory:G15} (добавлено {addVal:G15})");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Некорректный формат числа для M+");
                    }
                    break;

                case "M-":
                    Console.Write("Введите число для вычитания из памяти: ");
                    if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double subVal))
                    {
                        memory -= subVal;
                        Console.WriteLine($"Память: {memory:G15} (вычтено {subVal:G15})");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Некорректный формат числа для M-");
                    }
                    break;

                case "MR":
                    Console.WriteLine($"Значение из памяти: {memory:G15}");
                    break;

                default:
                    Console.WriteLine("Неизвестная команда памяти");
                    break;
            }
            Console.WriteLine(new string('-', 40));
        }
    }
}

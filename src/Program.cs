using example_screenshots.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace example_screenshots
{
    /// <summary>
    /// Author: Daniel Osborne
    /// Date: 2021-08-23
    /// Description: A small console application that takes a screenshot of the chosen window.
    /// Repo: https://github.com/MrDKOz/example-screenshot
    /// </summary>
    class Program
    {
        private static Process[] processes = Process.GetProcesses();
        private static List<string> availableWindows = FetchAvailableWindows();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Screenshot example!");

            var titleOfChosenWindow = ShowAvailableWindows();

            var handleOfChosenWindow = FetchTheHandleOfChosenWindow(titleOfChosenWindow);

            ScreenshotTheChosenWindow(handleOfChosenWindow);
        }

        /// <summary>
        /// Fetch all available processes and filter out those that have a window.
        /// </summary>
        /// <returns>A List<string> containing all window titles.</string></returns>
        static List<string> FetchAvailableWindows()
        {
            var availableWindows = new List<string>();

            foreach (var process in processes)
            {
                if (!string.IsNullOrWhiteSpace(process.MainWindowTitle))
                    availableWindows.Add(process.MainWindowTitle);
            }

            return availableWindows;
        }

        /// <summary>
        /// Displays all Windows that we can take screenshots of.
        /// </summary>
        /// <param name="availableWindows">A list of the titles for the available windows.</param>
        static string ShowAvailableWindows()
        {
            Console.WriteLine("=================");
            Console.WriteLine("Available Windows");
            Console.WriteLine("=================");

            for (int i = 0; i < availableWindows.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {availableWindows[i]}");
            }

            Console.WriteLine("=================");

            Console.WriteLine($"Which window would you like to screenshot? (1-{availableWindows.Count})");

            string selectedWindowTitle = string.Empty;

            do {
                var selection = Console.ReadLine();

                // Were we given a valid integer?
                if (!int.TryParse(selection, out int result))
                {
                    Console.WriteLine($"'{selection}' is not a valid selection, stop trying to break the example!");
                    continue;
                }

                // Was the integer within the allowed range?
                if (result > 0 && result <= availableWindows.Count)
                    selectedWindowTitle = availableWindows[result];
                else
                    Console.WriteLine($"Selection must be between 1 and {availableWindows.Count}, try again");

            } while (string.IsNullOrWhiteSpace(selectedWindowTitle));

            return selectedWindowTitle;
        }

        /// <summary>
        /// Fetch the MainWindowHandle for the window with the matching title.
        /// </summary>
        /// <param name="windowTitle">The title of the window to capture.</param>
        /// <returns>The MainWindowHandle of the chosen window.</returns>
        static IntPtr FetchTheHandleOfChosenWindow(string windowTitle)
        {
            foreach (var process in processes)
            {
                if (process.MainWindowTitle == windowTitle)
                    return process.MainWindowHandle;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Take a screenshot of the window with the given handle.
        /// </summary>
        /// <param name="handleOfWindow">The handle of the window to take the screenshot of.</param>
        static void ScreenshotTheChosenWindow(IntPtr handleOfWindow)
        {
            var window = new WindowModel(handleOfWindow);

            var screenshotImage = window.Screenshot();
            var fileSaveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}.bmp");

            Console.WriteLine("==========================================");
            Console.WriteLine("We have a screenshot of the chosen window!");
            Console.WriteLine($"Location: ({fileSaveLocation})");
            Console.WriteLine("==========================================");

            screenshotImage.Save(fileSaveLocation);
        }
    }
}

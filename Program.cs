using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Erreur : Veuillez spécifier un nom de projet.");
            return;
        }

        string projectName = args[0];

        // Étape 1: Créer un nouveau projet avec Vite
        RunCommand($"npm create vite@latest {projectName} -- --template vanilla");

        // Déplacer dans le dossier du projet
        string projectPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), projectName);
        Directory.SetCurrentDirectory(projectPath);

        // Étape 2: Installer Tailwind CSS, PostCSS, et Autoprefixer
        RunCommand("npm install -D tailwindcss postcss autoprefixer");

        // Étape 3: Initialiser les fichiers de configuration de Tailwind CSS
        RunCommand("npx tailwindcss init -p");

        // Étape 4: Configurer tailwind.config.js
        string tailwindConfigPath = System.IO.Path.Combine(projectPath, "tailwind.config.js");
        string tailwindConfigContent = @"
module.exports = {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {},
  },
  plugins: [],
};
";
        File.WriteAllText(tailwindConfigPath, tailwindConfigContent);
        Console.WriteLine("Fichier tailwind.config.js mis à jour.");

        // Étape 5: Ajouter les directives Tailwind dans le fichier CSS principal
        string cssPath = System.IO.Path.Combine(projectPath, "src", "style.css");
        string tailwindDirectives = @"
                @tailwind base;
                @tailwind components;
                @tailwind utilities;
            ";
        File.WriteAllText(cssPath, tailwindDirectives);
        Console.WriteLine("Directives Tailwind ajoutées au fichier CSS.");

        Console.WriteLine("Projet initialisé avec succès !");
    }

    static void RunCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;

        using (Process process = Process.Start(processStartInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}

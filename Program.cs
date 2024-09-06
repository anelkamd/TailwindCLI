using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Demander à l'utilisateur de saisir le nom du projet
        Console.WriteLine("Veuillez entrer le nom de votre projet :");
        string projectName = Console.ReadLine();

        // Vérifier si le nom du projet est vide
        if (string.IsNullOrEmpty(projectName))
        {
            Console.WriteLine("Erreur : Le nom du projet ne peut pas être vide.");
            return;
        }

        // Étape 1: Créer un nouveau projet avec Vite
        RunCommand($"npm create vite@latest {projectName} -- --template vanilla");

        // Déplacer dans le dossier du projet
        string projectPath = Path.Combine(Directory.GetCurrentDirectory(), projectName);
        Directory.SetCurrentDirectory(projectPath);

        // Étape 2: Installer Tailwind CSS, PostCSS, et Autoprefixer
        RunCommand("npm install -D tailwindcss postcss autoprefixer");

        // Étape 3: Initialiser les fichiers de configuration de Tailwind CSS
        RunCommand("npx tailwindcss init -p");

        // Étape 4: Configurer tailwind.config.js
        string tailwindConfigPath = Path.Combine(projectPath, "tailwind.config.js");
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
        string cssPath = Path.Combine(projectPath, "src", "style.css");
        string tailwindDirectives = @"
@tailwind base;
@tailwind components;
@tailwind utilities;
";
        File.WriteAllText(cssPath, tailwindDirectives);
        Console.WriteLine("Directives Tailwind ajoutées au fichier CSS.");

        Console.WriteLine("Projet initialisé avec succès !");
    }

    // Méthode pour exécuter une commande shell
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

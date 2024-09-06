static void Main(string[] args)
{
    Console.WriteLine("Début de l'exécution...");

    Console.WriteLine("Veuillez entrer le nom de votre projet :");
    string projectName = Console.ReadLine();

    if (string.IsNullOrEmpty(projectName))
    {
        Console.WriteLine("Erreur : Le nom du projet ne peut pas être vide.");
        return;
    }

    try
    {
        Console.WriteLine($"Création du projet : {projectName}");
        RunCommand($"npm create vite@latest {projectName} -- --template vanilla");

        Console.WriteLine("Changement de répertoire...");
        string projectPath = Path.Combine(Directory.GetCurrentDirectory(), projectName);
        Directory.SetCurrentDirectory(projectPath);

        Console.WriteLine("Installation de Tailwind CSS...");
        RunCommand("npm install -D tailwindcss postcss autoprefixer");

        Console.WriteLine("Initialisation de Tailwind CSS...");
        RunCommand("npx tailwindcss init -p");

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
    catch (Exception ex)
    {
        Console.WriteLine("Une erreur est survenue : " + ex.Message);
    }
}

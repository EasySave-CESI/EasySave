namespace EasySave.MVVM.Models
{
    public class LanguageConfiguration
    {
        private Dictionary<string, string> printStrings_en;
        private Dictionary<string, string> printStrings_fr;

        public LanguageConfiguration()
        {
            printStrings_en = new Dictionary<string, string>();
            printStrings_fr = new Dictionary<string, string>();

            LoadPrintStringsEn();
            LoadPrintStringsFr();
        }

        public Dictionary<string, string> GetPrintStringsEn()
        {
            return printStrings_en;
        }

        public Dictionary<string, string> GetPrintStringsFr()
        {
            return printStrings_fr;
        }

        private void LoadPrintStringsEn()
        {
            printStrings_en.Add("Separator", "--------------------------------------------------");
            printStrings_en.Add("dots", "...");
            printStrings_en.Add("WelcomeMessage", "Welcome to EasySave version: ");
            printStrings_en.Add("ArgumentError", "Error: Argument not recognized");
            printStrings_en.Add("DisplaySelectedProfileName", "You have selected the save profile: ");
            printStrings_en.Add("DisplayChooseSelectedProfile", "Select the save profile you want to modify by entering its index");
            printStrings_en.Add("Error", "Error: ");
            printStrings_en.Add("DisplayMenu_Header", "Please choose an option:");
            printStrings_en.Add("DisplayMenu_DislaySaveProfiles", "1. Display the save profiles");
            printStrings_en.Add("DisplayMenu_CreateSaveProfile", "x. Create a save profile");
            printStrings_en.Add("DisplayMenu_ModifySaveProfile", "2. Modify a save profile");
            printStrings_en.Add("DisplayMenu_ExecuteSaveProfile", "3. Execute a save profile");
            printStrings_en.Add("DisplayMenu_DisplayLogs", "4. Display the logs");
            printStrings_en.Add("DisplayMenu_Help", "5. Help");
            printStrings_en.Add("DisplayMenu_Configuration", "6. Configuration");
            printStrings_en.Add("DisplayMenu_Clear", "7. Clear the console");
            printStrings_en.Add("DisplayMenu_Exit", "8. Exit");
            printStrings_en.Add("DisplayMenuError", "Please enter a valid option.");
            printStrings_en.Add("DisplaySaveProfiles_Name", "Name:             ");
            printStrings_en.Add("DisplaySaveProfiles_SourceFilePath", "SourceFilePath:   ");
            printStrings_en.Add("DisplaySaveProfiles_TargetFilePath", "TargetFilePath:   ");
            printStrings_en.Add("DisplaySaveProfiles_State", "State:            ");
            printStrings_en.Add("DisplaySaveProfiles_TotalFilesToCopy", "TotalFilesToCopy: ");
            printStrings_en.Add("DisplaySaveProfiles_TotalFilesSize", "TotalFilesSize:   ");
            printStrings_en.Add("DisplaySaveProfiles_NbFilesLeftToDo", "NbFilesLeftToDo:  ");
            printStrings_en.Add("DisplaySaveProfiles_Progression", "Progression:      ");
            printStrings_en.Add("DisplaySaveProfiles_TypeOfSave", "TypeOfSave:       ");
            printStrings_en.Add("DisplaySaveProfilesError", "There is no save profile.");
            printStrings_en.Add("DisplayModifySaveProfileNewName", "Please enter the new name of the save profile:");
            printStrings_en.Add("DisplayModifySaveProfileNewSourceFilePath", "Please enter the new source file path of the save profile:");
            printStrings_en.Add("DisplayModifySaveProfileNewTargetFilePath", "Please enter the new target file path of the save profile:");
            printStrings_en.Add("DisplayModifySaveProfileNewTypeOfSave", "Please enter the new type of save of the save profile (full or diff):");
            printStrings_en.Add("DisplayModifySaveProfileSuccess", "The save profile has been modified successfully.");
            printStrings_en.Add("DisplayBackupInProgress", "Backing up profile: ");
            printStrings_en.Add("DisplayExecuteSaveProfileSuccess", "The save profile has been executed successfully.");
            printStrings_en.Add("DisplayExecuteSaveProfileStateError", "Error: The profile is not ready");
            printStrings_en.Add("DisplayExecuteSaveProfileTypeOfSaveError", "Error: The type of save is not valid");
            printStrings_en.Add("DisplayLogsHeader", "--------------- LOGS ---------------");
            printStrings_en.Add("DisplayLog_Name", "Name:             ");
            printStrings_en.Add("DisplayLog_SourceFilePath", "SourceFilePath:   ");
            printStrings_en.Add("DisplayLog_TargetFilePath", "TargetFilePath:   ");
            printStrings_en.Add("DisplayLog_FileSize", "FileSize:         ");
            printStrings_en.Add("DisplayLog_FileTransferTime", "FileTransferTime: ");
            printStrings_en.Add("DisplayLog_Time", "Time:             ");
            printStrings_en.Add("DisplayLogError", "There is no log.");
            printStrings_en.Add("Help_Header", "--------------- HELP ---------------");
            printStrings_en.Add("Help_Menu", "menu:     Display the menu");
            printStrings_en.Add("Help_DislaySaveProfiles", "dsp:      Display the save profiles");
            printStrings_en.Add("Help_CreateSaveProfile", "csp:      Create a save profile");
            printStrings_en.Add("Help_ModifySaveProfile", "msp:      Modify a save profile");
            printStrings_en.Add("Help_ExecuteSaveProfile", "esp:      Execute a save profile");
            printStrings_en.Add("Help_DisplayLogs", "dl:       Display the logs");
            printStrings_en.Add("Help_Help", "help:     Display the help");
            printStrings_en.Add("Help_Configuration", "config:   Display the configuration");
            printStrings_en.Add("Help_Exit", "exit:     Exit the program");
            printStrings_en.Add("DisplayConfigurationMenu_Header", "Please choose an option:");
            printStrings_en.Add("DisplayConfigurationMenu_DisplayLanguage", "The actual language is: ");
            printStrings_en.Add("DisplayConfigurationMenu_DisplayLogFormat", "The actual format of the log is: ");
            printStrings_en.Add("DisplayConfigurationMenu_ChangeLanguage", "1. Change the language");
            printStrings_en.Add("DisplayConfigurationMenu_ChangeLogFormat", "2. Change the format of the log");
            printStrings_en.Add("DisplayLanguageMenu_Header", "Please choose a language:");
            printStrings_en.Add("DisplayLanguageMenu_French", "1. French");
            printStrings_en.Add("DisplayLanguageMenu_English", "2. English");
            printStrings_en.Add("DisplayLanguageSuccess", "Language set to ");
            printStrings_en.Add("DisplayLanguageError", "Please enter a valid option for language.");
            printStrings_en.Add("DisplayLogFileFormatMenu_Header", "Please choose a format for the log:");
            printStrings_en.Add("DisplayLogFileFormatMenu_Json", "1. Json");
            printStrings_en.Add("DisplayLogFileFormatMenu_Xml", "2. Xml");
            printStrings_en.Add("DisplayLogFileFormatSuccess", "Format of the log set to ");
            printStrings_en.Add("DisplayLogFileFormatError", "Please enter a valid option for the format of the log.");
            printStrings_en.Add("DisplayConfigurationMenu_Back", "3. Back");
            printStrings_en.Add("Exit", "Thank you for using EasySave");
            printStrings_en.Add("NotImplementedYet", "Not implemented yet.");
        }

        private void LoadPrintStringsFr()
        {
            printStrings_fr.Add("Separator", "--------------------------------------------------");
            printStrings_fr.Add("dots", "...");
            printStrings_fr.Add("WelcomeMessage", "Bienvenue dans EasySave version: ");
            printStrings_fr.Add("ArgumentError", "Erreur : Argument non reconnu");
            printStrings_fr.Add("DisplaySelectedProfileName", "Vous avez sélectionné le profil de sauvegarde : ");
            printStrings_fr.Add("DisplayChooseSelectedProfile", "Sélectionnez le profil de sauvegarde que vous souhaitez modifier en entrant son indice");
            printStrings_fr.Add("Error", "Erreur : ");
            printStrings_fr.Add("DisplayMenu_Header", "Veuillez choisir une option :");
            printStrings_fr.Add("DisplayMenu_DislaySaveProfiles", "1. Afficher les profils de sauvegarde");
            printStrings_fr.Add("DisplayMenu_CreateSaveProfile", "x. Créer un profil de sauvegarde");
            printStrings_fr.Add("DisplayMenu_ModifySaveProfile", "2. Modifier un profil de sauvegarde");
            printStrings_fr.Add("DisplayMenu_ExecuteSaveProfile", "3. Exécuter un profil de sauvegarde");
            printStrings_fr.Add("DisplayMenu_DisplayLogs", "4. Afficher les registres");
            printStrings_fr.Add("DisplayMenu_Help", "5. Aide");
            printStrings_fr.Add("DisplayMenu_Configuration", "6. Configuration");
            printStrings_fr.Add("DisplayMenu_Clear", "7. Effacer la console");
            printStrings_fr.Add("DisplayMenu_Exit", "8. Quitter");
            printStrings_fr.Add("DisplayMenuError", "Veuillez entrer une option valide.");
            printStrings_fr.Add("DisplaySaveProfiles_Name", "Nom :               ");
            printStrings_fr.Add("DisplaySaveProfiles_SourceFilePath", "CheminSource :      ");
            printStrings_fr.Add("DisplaySaveProfiles_TargetFilePath", "CheminCible :       ");
            printStrings_fr.Add("DisplaySaveProfiles_State", "État :              ");
            printStrings_fr.Add("DisplaySaveProfiles_TotalFilesToCopy", "TotalFichiers :     ");
            printStrings_fr.Add("DisplaySaveProfiles_TotalFilesSize", "TailleTotale :      ");
            printStrings_fr.Add("DisplaySaveProfiles_NbFilesLeftToDo", "NbFichiersRestants: ");
            printStrings_fr.Add("DisplaySaveProfiles_Progression", "Progression :       ");
            printStrings_fr.Add("DisplaySaveProfiles_TypeOfSave", "TypeDeSauvegarde :  ");
            printStrings_fr.Add("DisplaySaveProfilesError", "Il n'y a aucun profil de sauvegarde.");
            printStrings_fr.Add("DisplayModifySaveProfileNewName", "Veuillez entrer le nouveau nom du profil de sauvegarde :");
            printStrings_fr.Add("DisplayModifySaveProfileNewSourceFilePath", "Veuillez entrer le nouveau chemin source du profil de sauvegarde :");
            printStrings_fr.Add("DisplayModifySaveProfileNewTargetFilePath", "Veuillez entrer le nouveau chemin cible du profil de sauvegarde :");
            printStrings_fr.Add("DisplayModifySaveProfileNewTypeOfSave", "Veuillez entrer le nouveau type de sauvegarde du profil (full ou diff) :");
            printStrings_fr.Add("DisplayModifySaveProfileSuccess", "Le profil de sauvegarde a été modifié avec succès.");
            printStrings_fr.Add("DisplayBackupInProgress", "Sauvegarde en cours pour le profil : ");
            printStrings_fr.Add("DisplayExecuteSaveProfileSuccess", "Le profil de sauvegarde a été exécuté avec succès.");
            printStrings_fr.Add("DisplayExecuteSaveProfileStateError", "Erreur : Le profil n'est pas prêt");
            printStrings_fr.Add("DisplayExecuteSaveProfileTypeOfSaveError", "Erreur : Le type de sauvegarde n'est pas valide");
            printStrings_fr.Add("DisplayLogsHeader", "--------------- REGISTRES ---------------");
            printStrings_fr.Add("DisplayLog_Name", "Nom :             ");
            printStrings_fr.Add("DisplayLog_SourceFilePath", "CheminSource :    ");
            printStrings_fr.Add("DisplayLog_TargetFilePath", "CheminCible :     ");
            printStrings_fr.Add("DisplayLog_FileSize", "TailleFichier :    ");
            printStrings_fr.Add("DisplayLog_FileTransferTime", "TempsTransfertFichier : ");
            printStrings_fr.Add("DisplayLog_Time", "Temps :            ");
            printStrings_fr.Add("DisplayLogError", "Il n'y a aucun registre.");
            printStrings_fr.Add("Help_Header", "--------------- AIDE ---------------");
            printStrings_fr.Add("Help_Menu", "menu :    Afficher le menu");
            printStrings_fr.Add("Help_DislaySaveProfiles", "dsp :     Afficher les profils de sauvegarde");
            printStrings_fr.Add("Help_CreateSaveProfile", "csp :     Créer un profil de sauvegarde");
            printStrings_fr.Add("Help_ModifySaveProfile", "msp :     Modifier un profil de sauvegarde");
            printStrings_fr.Add("Help_ExecuteSaveProfile", "esp :     Exécuter un profil de sauvegarde");
            printStrings_fr.Add("Help_DisplayLogs", "dl :      Afficher les registres");
            printStrings_fr.Add("Help_Help", "help :    Afficher l'aide");
            printStrings_fr.Add("Help_Configuration", "config :  Afficher la configuration");
            printStrings_fr.Add("Help_Exit", "exit :    Quitter le programme");
            printStrings_fr.Add("DisplayConfigurationMenu_Header", "Veuillez choisir une option :");
            printStrings_fr.Add("DisplayConfigurationMenu_DisplayLanguage", "La langue actuelle est : ");
            printStrings_fr.Add("DisplayConfigurationMenu_DisplayLogFormat", "Le format actuel du journal est : ");
            printStrings_fr.Add("DisplayConfigurationMenu_ChangeLanguage", "1. Changer la langue");
            printStrings_fr.Add("DisplayConfigurationMenu_ChangeLogFormat", "2. Changer le format du journal");
            printStrings_fr.Add("DisplayLanguageMenu_Header", "Veuillez choisir une langue :");
            printStrings_fr.Add("DisplayLanguageMenu_French", "1. Français");
            printStrings_fr.Add("DisplayLanguageMenu_English", "2. Anglais");
            printStrings_fr.Add("DisplayLanguageSuccess", "Langue définie sur ");
            printStrings_fr.Add("DisplayLanguageError", "Veuillez entrer une option valide pour la langue.");
            printStrings_fr.Add("DisplayLogFileFormatMenu_Header", "Veuillez choisir un format pour le journal :");
            printStrings_fr.Add("DisplayLogFileFormatMenu_Json", "1. Json");
            printStrings_fr.Add("DisplayLogFileFormatMenu_Xml", "2. Xml");
            printStrings_fr.Add("DisplayLogFileFormatSuccess", "Format du journal défini sur ");
            printStrings_fr.Add("DisplayLogFileFormatError", "Veuillez entrer une option valide pour le format du journal.");
            printStrings_fr.Add("DisplayConfigurationMenu_Back", "3. Retour");
            printStrings_fr.Add("Exit", "Merci d'utiliser EasySave");
            printStrings_fr.Add("NotImplementedYet", "Pas encore implémenté.");
        }
    }
}

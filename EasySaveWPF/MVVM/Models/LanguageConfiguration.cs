using System.CodeDom.Compiler;

namespace EasySaveWPF.MVVM.Models
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
            // Console
            printStrings_en.Add("Console_Separator", "--------------------------------------------------");
            printStrings_en.Add("Console_Dots", "...");
            printStrings_en.Add("Console_WelcomeMessage", "Welcome to EasySave version: ");
            printStrings_en.Add("Console_ArgumentError", "Error: Argument not recognized");
            printStrings_en.Add("Console_DisplaySelectedProfileName", "You have selected the save profile: ");
            printStrings_en.Add("Console_DisplayChooseSelectedProfile", "Select the save profile you want to modify by entering its index");
            printStrings_en.Add("Console_Error", "Error: ");
            printStrings_en.Add("Console_DisplayMenu_Header", "Please choose an option:");
            printStrings_en.Add("Console_DisplayMenu_DislaySaveProfiles", "1. Display the save profiles");
            printStrings_en.Add("Console_DisplayMenu_CreateSaveProfile", "x. Create a save profile");
            printStrings_en.Add("Console_DisplayMenu_ModifySaveProfile", "2. Modify a save profile");
            printStrings_en.Add("Console_DisplayMenu_ExecuteSaveProfile", "3. Execute a save profile");
            printStrings_en.Add("Console_DisplayMenu_DisplayLogs", "4. Display the logs");
            printStrings_en.Add("Console_DisplayMenu_Help", "5. Help");
            printStrings_en.Add("Console_DisplayMenu_Configuration", "6. Configuration");
            printStrings_en.Add("Console_DisplayMenu_Clear", "7. Clear the console");
            printStrings_en.Add("Console_DisplayMenu_Exit", "8. Exit");
            printStrings_en.Add("Console_DisplayMenuError", "Please enter a valid option.");
            printStrings_en.Add("Console_DisplaySaveProfiles_Name", "Name:             ");
            printStrings_en.Add("Console_DisplaySaveProfiles_SourceFilePath", "SourceFilePath:   ");
            printStrings_en.Add("Console_DisplaySaveProfiles_TargetFilePath", "TargetFilePath:   ");
            printStrings_en.Add("Console_DisplaySaveProfiles_State", "State:            ");
            printStrings_en.Add("Console_DisplaySaveProfiles_TotalFilesToCopy", "TotalFilesToCopy: ");
            printStrings_en.Add("Console_DisplaySaveProfiles_TotalFilesSize", "TotalFilesSize:   ");
            printStrings_en.Add("Console_DisplaySaveProfiles_NbFilesLeftToDo", "NbFilesLeftToDo:  ");
            printStrings_en.Add("Console_DisplaySaveProfiles_Progression", "Progression:      ");
            printStrings_en.Add("Console_DisplaySaveProfiles_TypeOfSave", "TypeOfSave:       ");
            printStrings_en.Add("Console_DisplaySaveProfilesError", "There is no save profile.");
            printStrings_en.Add("Console_DisplayModifySaveProfileNewName", "Please enter the new name of the save profile:");
            printStrings_en.Add("Console_DisplayModifySaveProfileNewSourceFilePath", "Please enter the new source file path of the save profile:");
            printStrings_en.Add("Console_DisplayModifySaveProfileNewTargetFilePath", "Please enter the new target file path of the save profile:");
            printStrings_en.Add("Console_DisplayModifySaveProfileNewTypeOfSave", "Please enter the new type of save of the save profile (full or diff):");
            printStrings_en.Add("Console_DisplayModifySaveProfileSuccess", "The save profile has been modified successfully.");
            printStrings_en.Add("Console_DisplayBackupInProgress", "Backing up profile: ");
            printStrings_en.Add("Console_DisplayExecuteSaveProfileSuccess", "The save profile has been executed successfully.");
            printStrings_en.Add("Console_DisplayExecuteSaveProfileStateError", "Error: The profile is not ready");
            printStrings_en.Add("Console_DisplayExecuteSaveProfileTypeOfSaveError", "Error: The type of save is not valid");
            printStrings_en.Add("Console_DisplayLogsHeader", "--------------- LOGS ---------------");
            printStrings_en.Add("Console_DisplayLog_Name", "Name:             ");
            printStrings_en.Add("Console_DisplayLog_SourceFilePath", "SourceFilePath:   ");
            printStrings_en.Add("Console_DisplayLog_TargetFilePath", "TargetFilePath:   ");
            printStrings_en.Add("Console_DisplayLog_FileSize", "FileSize:         ");
            printStrings_en.Add("Console_DisplayLog_FileTransferTime", "FileTransferTime: ");
            printStrings_en.Add("Console_DisplayLog_Time", "Time:             ");
            printStrings_en.Add("Console_DisplayLogError", "There is no log.");
            printStrings_en.Add("Console_Help_Header", "--------------- HELP ---------------");
            printStrings_en.Add("Console_Help_Menu", "menu:     Display the menu");
            printStrings_en.Add("Console_Help_DislaySaveProfiles", "dsp:      Display the save profiles");
            printStrings_en.Add("Console_Help_CreateSaveProfile", "csp:      Create a save profile");
            printStrings_en.Add("Console_Help_ModifySaveProfile", "msp:      Modify a save profile");
            printStrings_en.Add("Console_Help_ExecuteSaveProfile", "esp:      Execute a save profile");
            printStrings_en.Add("Console_Help_DisplayLogs", "dl:       Display the logs");
            printStrings_en.Add("Console_Help_Help", "help:     Display the help");
            printStrings_en.Add("Console_Help_Configuration", "config:   Display the configuration");
            printStrings_en.Add("Console_Help_Exit", "exit:     Exit the program");
            printStrings_en.Add("Console_DisplayConfigurationMenu_Header", "Please choose an option:");
            printStrings_en.Add("Console_DisplayConfigurationMenu_DisplayLanguage", "The actual language is: ");
            printStrings_en.Add("Console_DisplayConfigurationMenu_DisplayLogFormat", "The actual format of the log is: ");
            printStrings_en.Add("Console_DisplayConfigurationMenu_ChangeLanguage", "1. Change the language");
            printStrings_en.Add("Console_DisplayConfigurationMenu_ChangeLogFormat", "2. Change the format of the log");
            printStrings_en.Add("Console_DisplayLanguageMenu_Header", "Please choose a language:");
            printStrings_en.Add("Console_DisplayLanguageMenu_French", "1. French");
            printStrings_en.Add("Console_DisplayLanguageMenu_English", "2. English");
            printStrings_en.Add("Console_DisplayLanguageSuccess", "Language set to ");
            printStrings_en.Add("Console_DisplayLanguageError", "Please enter a valid option for language.");
            printStrings_en.Add("Console_DisplayLogFileFormatMenu_Header", "Please choose a format for the log:");
            printStrings_en.Add("Console_DisplayLogFileFormatMenu_Json", "1. Json");
            printStrings_en.Add("Console_DisplayLogFileFormatMenu_Xml", "2. Xml");
            printStrings_en.Add("Console_DisplayLogFileFormatSuccess", "Format of the log set to ");
            printStrings_en.Add("Console_DisplayLogFileFormatError", "Please enter a valid option for the format of the log.");
            printStrings_en.Add("Console_DisplayConfigurationMenu_Back", "3. Back");
            printStrings_en.Add("Console_Exit", "Thank you for using EasySave");
            printStrings_en.Add("Console_NotImplementedYet", "Not implemented yet.");

            // ------------------     WPF     ------------------

            // Title
            printStrings_en.Add("Application_MainWindow_Title", "EasySave"); 

            // MainWindow Buttons
            printStrings_en.Add("Application_MainWindow_ManageProfile_Button", "Manage Profile");
            printStrings_en.Add("Application_MainWindow_ExecuteSave_Button", "Execute save(s)");
            printStrings_en.Add("Application_MainWindow_ViewLogs_Button", "View Logs");

            // MainWindow Labels
            printStrings_en.Add("Application_MainWindow_ListOfProfiles_Label", "List of save profiles :");
            printStrings_en.Add("Application_MainWindow_NumberOfprofiles_Label", "Number of profiles loaded :");
            printStrings_en.Add("Application_MainWindow_State_Label", "State :");

            // MainWindow Headers
            printStrings_en.Add("Application_MainWindow_Index_Header", "Index");
            printStrings_en.Add("Application_MainWindow_ProfileName_Header", "Name");
            printStrings_en.Add("Application_MainWindow_SourceFilePath_Header", "Source File Path");
            printStrings_en.Add("Application_MainWindow_TargetFilePath_Header", "Target File Path");
            printStrings_en.Add("Application_MainWindow_TypeOfSave_Header", "Type");
            printStrings_en.Add("Application_MainWindow_State_Header", "State");

            // ManageProfileWindow Buttons
            printStrings_en.Add("Application_ManageProfileView_Validate_Button", "Validate");
            printStrings_en.Add("Application_ManageProfileView_Exit_Button", "Exit");

            // ManageProfileWindow Labels
            printStrings_en.Add("Application_ManageProfileView_Source_Label", "Source :");
            printStrings_en.Add("Application_ManageProfileView_Destination_Label", "Destination :");

            printStrings_en.Add("Application_ManageProfileView_Type_Label", "Type :");
            printStrings_en.Add("Application_ManageProfileView_TypeFull_RadioButton", "full");
            printStrings_en.Add("Application_ManageProfileView_TypeDiff_RadioButton", "diff");

            printStrings_en.Add("Application_ManageProfileView_Encryption_Label", "Encryption :");
            printStrings_en.Add("Application_ManageProfileView_EncryptionYes_RadioButton", "yes");
            printStrings_en.Add("Application_ManageProfileView_EncryptionNo_RadioButton", "no");

            // ManageProfileWindow Header
            printStrings_en.Add("Application_ManageProfileView_Index_Header", "Index");
            printStrings_en.Add("Application_ManageProfileView_ProfileName_Header", "Name");
            printStrings_en.Add("Application_ManageProfileView_SourceFilePath_Header", "Source File Path");
            printStrings_en.Add("Application_ManageProfileView_DestinationFilePath_Header", "Target File Path");
            printStrings_en.Add("Application_ManageProfileView_TypeOfSave_Header", "Type");
            printStrings_en.Add("Application_ManageProfileView_State_Header", "State");

            // ExecuteSaveWindow
            printStrings_en.Add("Application_ExecuteSaveView_UserSelection_Label", "Saves :");
            printStrings_en.Add("Application_ExecuteSaveView_Start_Button", "Start");
            printStrings_en.Add("Application_ExecuteSaveView_Exit_Button", "Exit");

            // OptionWindow
            printStrings_en.Add("Application_OptionView_Validate_Button", "Validate");
            printStrings_en.Add("Application_OptionView_LogFormat_Label", "Log format :");
            printStrings_en.Add("Application_OptionView_Language_Label", "Language :");
        }

        private void LoadPrintStringsFr()
        {
            // Console
            printStrings_fr.Add("Console_Separator", "--------------------------------------------------");
            printStrings_fr.Add("Console_Dots", "...");
            printStrings_fr.Add("Console_WelcomeMessage", "Bienvenue dans EasySave version: ");
            printStrings_fr.Add("Console_ArgumentError", "Erreur : Argument non reconnu");
            printStrings_fr.Add("Console_DisplaySelectedProfileName", "Vous avez sélectionné le profil de sauvegarde : ");
            printStrings_fr.Add("Console_DisplayChooseSelectedProfile", "Sélectionnez le profil de sauvegarde que vous souhaitez modifier en entrant son indice");
            printStrings_fr.Add("Console_Error", "Erreur : ");
            printStrings_fr.Add("Console_DisplayMenu_Header", "Veuillez choisir une option :");
            printStrings_fr.Add("Console_DisplayMenu_DislaySaveProfiles", "1. Afficher les profils de sauvegarde");
            printStrings_fr.Add("Console_DisplayMenu_CreateSaveProfile", "x. Créer un profil de sauvegarde");
            printStrings_fr.Add("Console_DisplayMenu_ModifySaveProfile", "2. Modifier un profil de sauvegarde");
            printStrings_fr.Add("Console_DisplayMenu_ExecuteSaveProfile", "3. Exécuter un profil de sauvegarde");
            printStrings_fr.Add("Console_DisplayMenu_DisplayLogs", "4. Afficher les registres");
            printStrings_fr.Add("Console_DisplayMenu_Help", "5. Aide");
            printStrings_fr.Add("Console_DisplayMenu_Configuration", "6. Configuration");
            printStrings_fr.Add("Console_DisplayMenu_Clear", "7. Effacer la console");
            printStrings_fr.Add("Console_DisplayMenu_Exit", "8. Quitter");
            printStrings_fr.Add("Console_DisplayMenuError", "Veuillez entrer une option valide.");
            printStrings_fr.Add("Console_DisplaySaveProfiles_Name", "Nom :               ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_SourceFilePath", "CheminSource :      ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_TargetFilePath", "CheminCible :       ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_State", "État :              ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_TotalFilesToCopy", "TotalFichiers :     ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_TotalFilesSize", "TailleTotale :      ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_NbFilesLeftToDo", "NbFichiersRestants: ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_Progression", "Progression :       ");
            printStrings_fr.Add("Console_DisplaySaveProfiles_TypeOfSave", "TypeDeSauvegarde :  ");
            printStrings_fr.Add("Console_DisplaySaveProfilesError", "Il n'y a aucun profil de sauvegarde.");
            printStrings_fr.Add("Console_DisplayModifySaveProfileNewName", "Veuillez entrer le nouveau nom du profil de sauvegarde :");
            printStrings_fr.Add("Console_DisplayModifySaveProfileNewSourceFilePath", "Veuillez entrer le nouveau chemin source du profil de sauvegarde :");
            printStrings_fr.Add("Console_DisplayModifySaveProfileNewTargetFilePath", "Veuillez entrer le nouveau chemin cible du profil de sauvegarde :");
            printStrings_fr.Add("Console_DisplayModifySaveProfileNewTypeOfSave", "Veuillez entrer le nouveau type de sauvegarde du profil (full ou diff) :");
            printStrings_fr.Add("Console_DisplayModifySaveProfileSuccess", "Le profil de sauvegarde a été modifié avec succès.");
            printStrings_fr.Add("Console_DisplayBackupInProgress", "Sauvegarde en cours pour le profil : ");
            printStrings_fr.Add("Console_DisplayExecuteSaveProfileSuccess", "Le profil de sauvegarde a été exécuté avec succès.");
            printStrings_fr.Add("Console_DisplayExecuteSaveProfileStateError", "Erreur : Le profil n'est pas prêt");
            printStrings_fr.Add("Console_DisplayExecuteSaveProfileTypeOfSaveError", "Erreur : Le type de sauvegarde n'est pas valide");
            printStrings_fr.Add("Console_DisplayLogsHeader", "--------------- REGISTRES ---------------");
            printStrings_fr.Add("Console_DisplayLog_Name", "Nom :             ");
            printStrings_fr.Add("Console_DisplayLog_SourceFilePath", "CheminSource :    ");
            printStrings_fr.Add("Console_DisplayLog_TargetFilePath", "CheminCible :     ");
            printStrings_fr.Add("Console_DisplayLog_FileSize", "TailleFichier :    ");
            printStrings_fr.Add("Console_DisplayLog_FileTransferTime", "TempsTransfertFichier : ");
            printStrings_fr.Add("Console_DisplayLog_Time", "Temps :            ");
            printStrings_fr.Add("Console_DisplayLogError", "Il n'y a aucun registre.");
            printStrings_fr.Add("Console_Help_Header", "--------------- AIDE ---------------");
            printStrings_fr.Add("Console_Help_Menu", "menu :    Afficher le menu");
            printStrings_fr.Add("Console_Help_DislaySaveProfiles", "dsp :     Afficher les profils de sauvegarde");
            printStrings_fr.Add("Console_Help_CreateSaveProfile", "csp :     Créer un profil de sauvegarde");
            printStrings_fr.Add("Console_Help_ModifySaveProfile", "msp :     Modifier un profil de sauvegarde");
            printStrings_fr.Add("Console_Help_ExecuteSaveProfile", "esp :     Exécuter un profil de sauvegarde");
            printStrings_fr.Add("Console_Help_DisplayLogs", "dl :      Afficher les registres");
            printStrings_fr.Add("Console_Help_Help", "help :    Afficher l'aide");
            printStrings_fr.Add("Console_Help_Configuration", "config :  Afficher la configuration");
            printStrings_fr.Add("Console_Help_Exit", "exit :    Quitter le programme");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_Header", "Veuillez choisir une option :");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_DisplayLanguage", "La langue actuelle est : ");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_DisplayLogFormat", "Le format actuel du journal est : ");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_ChangeLanguage", "1. Changer la langue");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_ChangeLogFormat", "2. Changer le format du journal");
            printStrings_fr.Add("Console_DisplayLanguageMenu_Header", "Veuillez choisir une langue :");
            printStrings_fr.Add("Console_DisplayLanguageMenu_French", "1. Français");
            printStrings_fr.Add("Console_DisplayLanguageMenu_English", "2. Anglais");
            printStrings_fr.Add("Console_DisplayLanguageSuccess", "Langue définie sur ");
            printStrings_fr.Add("Console_DisplayLanguageError", "Veuillez entrer une option valide pour la langue.");
            printStrings_fr.Add("Console_DisplayLogFileFormatMenu_Header", "Veuillez choisir un format pour le journal :");
            printStrings_fr.Add("Console_DisplayLogFileFormatMenu_Json", "1. Json");
            printStrings_fr.Add("Console_DisplayLogFileFormatMenu_Xml", "2. Xml");
            printStrings_fr.Add("Console_DisplayLogFileFormatSuccess", "Format du journal défini sur ");
            printStrings_fr.Add("Console_DisplayLogFileFormatError", "Veuillez entrer une option valide pour le format du journal.");
            printStrings_fr.Add("Console_DisplayConfigurationMenu_Back", "3. Retour");
            printStrings_fr.Add("Console_Exit", "Merci d'utiliser EasySave");
            printStrings_fr.Add("Console_NotImplementedYet", "Pas encore implémenté.");

            // ------------------     WPF     ------------------

            // Title
            printStrings_fr.Add("Application_MainWindow_Title", "EasySave");

            // MainWindow Buttons
            printStrings_fr.Add("Application_MainWindow_ManageProfile_Button", "Gérer les profils");
            printStrings_fr.Add("Application_MainWindow_ExecuteSave_Button", "Exécuter les sauvegardes");
            printStrings_fr.Add("Application_MainWindow_ViewLogs_Button", "Voir les registres");

            // MainWindow Labels
            printStrings_fr.Add("Application_MainWindow_ListOfProfiles_Label", "Liste des profils de sauvegarde :");
            printStrings_fr.Add("Application_MainWindow_NumberOfprofiles_Label", "Nombre de profils chargés :");
            printStrings_fr.Add("Application_MainWindow_State_Label", "État :");

            // MainWindow Headers
            printStrings_fr.Add("Application_MainWindow_Index_Header", "Indice");
            printStrings_fr.Add("Application_MainWindow_ProfileName_Header", "Nom");
            printStrings_fr.Add("Application_MainWindow_SourceFilePath_Header", "Chemin Source");
            printStrings_fr.Add("Application_MainWindow_TargetFilePath_Header", "Chemin Cible");
            printStrings_fr.Add("Application_MainWindow_TypeOfSave_Header", "Type");
            printStrings_fr.Add("Application_MainWindow_State_Header", "État");

            // ManageProfileWindow Buttons
            printStrings_fr.Add("Application_ManageProfileView_Validate_Button", "Valider");
            printStrings_fr.Add("Application_ManageProfileView_Exit_Button", "Quitter");

            // ManageProfileWindow Labels
            printStrings_fr.Add("Application_ManageProfileView_Source_Label", "Source :");
            printStrings_fr.Add("Application_ManageProfileView_Destination_Label", "Destination :");

            printStrings_fr.Add("Application_ManageProfileView_Type_Label", "Type :");
            printStrings_fr.Add("Application_ManageProfileView_TypeFull_RadioButton", "full");
            printStrings_fr.Add("Application_ManageProfileView_TypeDiff_RadioButton", "diff");

            printStrings_fr.Add("Application_ManageProfileView_Encryption_Label", "Chiffrement :");
            printStrings_fr.Add("Application_ManageProfileView_EncryptionYes_RadioButton", "oui");
            printStrings_fr.Add("Application_ManageProfileView_EncryptionNo_RadioButton", "non");

            // ManageProfileWindow Header
            printStrings_fr.Add("Application_ManageProfileView_Index_Header", "Indice");
            printStrings_fr.Add("Application_ManageProfileView_ProfileName_Header", "Nom");
            printStrings_fr.Add("Application_ManageProfileView_SourceFilePath_Header", "Chemin Source");
            printStrings_fr.Add("Application_ManageProfileView_DestinationFilePath_Header", "Chemin Cible");
            printStrings_fr.Add("Application_ManageProfileView_TypeOfSave_Header", "Type");
            printStrings_fr.Add("Application_ManageProfileView_State_Header", "État");

            // ExecuteSaveWindow
            printStrings_fr.Add("Application_ExecuteSaveView_UserSelection_Label", "Sauvegardes :");
            printStrings_fr.Add("Application_ExecuteSaveView_Start_Button", "Démarrer");
            printStrings_fr.Add("Application_ExecuteSaveView_Exit_Button", "Quitter");

            // OptionWindow
            printStrings_fr.Add("Application_OptionView_Validate_Button", "Valider");
            printStrings_fr.Add("Application_OptionView_LogFormat_Label", "Format du journal :");
            printStrings_fr.Add("Application_OptionView_Language_Label", "Langue :");
        }
    }
}

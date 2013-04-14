MODIFICATION EFFECTUEE POUR LA VERSION 2 (Azure) :

-Ajout du projet CloudService "Coombi125565Cloud"
-Ajout du projet WorkerRole "Coombi125565Worker"

-Modification de la chaine de connexion sql : -> vers serveur sql Azure

-Possibilité d'associer une image à un post
	--stockée sous forme de blob
	--l'image est automatiquement redimensionné par le worker role.

-Les images sont affichés en thumbnail et sont un lien vers l'image de taille d'origine.


DEPLOIEMENT VERS AURE CLOUD SERVICE : http://betoucloud.cloudapp.net
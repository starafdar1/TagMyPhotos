# TagMyPhotos
A simple console application for tagging photos with the persons in the photos using the Microsoft Face Azure Service.

A subscription to the Microsoft Face Azure Service is required for running this. There is a cost associated with calling the Face service. Pricing details are available in the Microsoft Azure Site.

The application first needs to be trained using a few sample photos of each person to be identified. A separate folder should be created for each person containing images of the person. Typically the folder name would be the name of the person (without any spaces). 

After the training is done, the application can tag the photos. There are 2 options for tagging the photos - show the persons in each photo in the console output, or rename the image files to contain the name of the persons. 

The parameters to the application are as follows: 

        /endpoint         : The endpoint for a Microsoft Face AI Service. (String) (Required)
        /operationmode    : Indicates whether we are training the engine, or using the engine to tag images. (OperationModes (Tag/Train)) (Default = Tag)
        /photosgroupid    : An Id for the group of faces to be identified. Use the same Id during training and tagging mode. (String) (Required) (Matches regex: '[0-9a-z_]+')
        /samplephotospath : Path to the folder containing the sample images of persons to be identified. This folder should contain one subfolder for each person to be identified. The subfolders should contain sample images for that person. These images will be used for training the face recognition engine. (String) (Required)
        /subscriptionkey  : The Subscription Key for a Microsoft Face AI Service. (String) (Required)
        /tagmode          : Indicates what the result of the tagging should be. ConsoleOutput will show the names of the persons found in each photo in the console. RenameFiles will rename the original files by appending the names of the persons found in the photo to the filename. (TagModes (CosoleOutput/RenameFiles)) (Default = CosoleOutput)
        /tagphotospath    : Path to the folder containing the images to be tagged. (String) (Required)
        
The parameters should be in the format **-name=value**. For example, 
>-operationMode="Tag"

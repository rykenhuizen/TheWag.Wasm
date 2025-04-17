# TheWag Solution

## Overview

TheWag is a Blazor WebAssembly application that allows users to upload and sell pictures of dogs. The application uses Azure Functions and Azure Cognitive Services to analyze the uploaded images and determine if they are pictures of dogs. If the image is identified as a dog, the user can proceed to set a price and stock for the image and add it to the catalog.

## Key Components

## Blazor WebAssembly Front-End

1. **Pages/Sell.razor**:
   - This page allows users to upload images, analyze them, and sell them if they are identified as dog pictures.
   - It includes various stages such as ready to upload, ready to sell, added to catalog, and not a dog.

2. **Pages/Cart.razor**:
   - This page displays the shopping cart where users can see the items they have added.
   - It allows users to increment or decrement the quantity of items in the cart.
   - Displays the total price of the items in the cart.
  
3. **Pages/Catalog.razor**:
   - This page displays a catalog of dog pictures available for sale.
   - It fetches the list of products from the server and displays them in a grid or list format.
   - Users can view details of each product and add them to their shopping cart.

4. **Services/ProductService.cs**:
   - This service handles the interaction with Azure Functions for image analysis and saving product details.
   - It includes methods for getting all products, saving a product, getting a list of blobs, and analyzing an image.

5. **Services/CartService.cs**:
   - This service manages the shopping cart functionality.
   - It includes methods for adding, removing, and updating items in the cart.


## Azure Functions

1. **Blob Functions**:
   - These functions handle operations related to Azure Blob Storage.
   - They include functionality for uploading, moving, and deleting blobs.
   - Used for storing and managing images uploaded by users.

2. **Data Functions**:
   - These functions handle data operations such as saving and retrieving product details.
   - They interact with a database to store and manage product information.
   - Include functions for creating, reading, updating, and deleting product records.

3. **Vision Functions**:
   - These functions interact with Azure Cognitive Services to analyze images.
   - They include functionality for sending images to the Computer Vision API and processing the results.
   - Used to determine if an uploaded image is a picture of a dog and extract relevant tags and descriptions.

## Azure Cognitive Services

1. **Util.Azure.ComputerVisionClient/ComputerVisionClient.cs**:
   - This class interacts with Azure Cognitive Services to analyze images.
   - It uses the `ImageAnalysisClient` to get image analysis results such as captions and tags.
   - It checks if the image is a picture of a dog and returns the analysis results.

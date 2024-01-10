# Use an official Node.js runtime as a base image
FROM node:18-alpine as builder

# Set the working directory inside the container
WORKDIR /app
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the remaining application files to the working directory
COPY . .

# Build the React app
RUN npm run build

# Expose the port that the app will run on
EXPOSE 3000

# Define the command to run the application
CMD ["npm", "start"]
pipeline {
    agent {
        label 'windows'  // Make sure your Jenkins agent runs on Windows
    }
    stages {
        stage('Checkout') {
            steps {
                // Checkout the code from your GitHub repo
                git 'https://github.com/Aamantamboli/Dotnetapi.git'
            }
        }
        stage('Restore Dependencies') {
            steps {
                // Restore NuGet packages
                bat 'dotnet restore KubernetesAutoClusterAPI/KubernetesAutoClusterAPI.csproj'
            }
        }
        // Uncomment this stage if you want to build the project
        // stage('Build') {
        //     steps {
        //         // Build the project
        //         bat 'dotnet build KubernetesAutoClusterAPI/KubernetesAutoClusterAPI.csproj -c Release'
        //     }
        // }
        stage('Publish') {
            steps {
                // Publish the API for win-x64
                bat 'dotnet publish KubernetesAutoClusterAPI/KubernetesAutoClusterAPI.csproj -c Release -r win-x64 --self-contained false -o "C:\\Jenkins\\publish\\"'
            }
        }
        stage('Deploy to IIS') {
            steps {
                script {
                    // Define the source and destination paths
                    def sourceDir = 'C:\\Jenkins\\publish\\'
                    def targetDir = 'C:\\inetpub\\wwwroot\\KubernetesAutoClusterAPI'

                    // Clean up the target directory (optional)
                    // bat "rmdir /S /Q \"${targetDir}\""
                    
                    // Create the target directory if it doesn't exist
                    bat "mkdir \"${targetDir}\""

                    // Copy the published files to the IIS path
                    bat "xcopy \"${sourceDir}\\*\" \"${targetDir}\" /E /I /Y"
                }
            }
        }
    }
    post {
        success {
            echo 'Deployment completed successfully!'
        }
        failure {
            echo 'Deployment failed!'
        }
    }
}

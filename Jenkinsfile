pipeline {
    agent { label 'windows' }


    stages {
        stage('Clean Workspace') {
            steps {
                cleanWs() // Clean the workspace before starting
            }
        }
        
        stage('Checkout Code') {
            steps {
                // Clone the repository into the wwwroot directory
                dir('C:\\inetpub\\wwwroot') {
                    git branch: 'master', url: 'https://github.com/Aamantamboli/Dotnetapi.git'
                }
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Restore NuGet packages for the .csproj file in KubernetesAutoClusterAPI
                bat 'dotnet restore C:\\inetpub\\wwwroot\\Dotnetapi\\KubernetesAutoClusterAPI\\'
            }
        }

        stage('Build and Publish') {
            steps {
                // Build and publish the project with x64 runtime to the specified folder
                bat 'dotnet publish C:\\inetpub\\wwwroot\\Dotnetapi\\KubernetesAutoClusterAPI\\ -c Release -r win-x64 --self-contained -o C:\\inetpub\\wwwroot\\published-app'
            }
        }

        stage('Deploy to IIS') {
            steps {
                // This step will ensure the API is deployed to IIS
                powershell '''
                Stop-WebAppPool -Name "DefaultAppPool"
                Start-WebAppPool -Name "WebAppPool"
                '''
                // Optional: Move published files to wwwroot folder or deploy to IIS
                // Replace this section with any IIS deployment commands if necessary
                echo 'Deploying application to IIS...'
                // bat 'xcopy /s /e /y C:\\inetpub\\wwwroot\\published-app\\* C:\\inetpub\\wwwroot\\'
            }
        }
    }

    post {
        always {
            cleanWs() // Clean up workspace after build
        }
    }
}

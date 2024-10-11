pipeline {
    agent { label 'windows' } // Replace 'windows-node' with the label of your Windows EC2 node
    environment {
        // Define paths and environment variables
        IIS_WEBSITE_PATH = 'C:\\inetpub\\wwwroot\\workspace'
    }
    stages {
        stage('Checkout Code') {
            steps {
                // Checkout the code from your repository
                git branch: 'master', url: 'https://github.com/Aamantamboli/Dotnetapi.git', credentialsId: 'windows'
            }
        }   
        stage('Restore Dependencies') {
            steps {
                // Use MSBuild to restore dependencies
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" restore'
            }
        }

        
        stage('Build') {
            steps {
                // Build the .NET API project
                bat 'dotnet build --configuration Release'
            }
        }
        
        stage('Publish') {
            steps {
                // Publish the .NET API to the IIS folder
                bat 'dotnet publish -c Release -r win-x64 --self-contained false -o ${env.IIS_WEBSITE_PATH}'
            }
        }
        
        stage('Deploy to IIS') {
            steps {
                // This step will ensure the API is deployed to IIS
                bat '''
                Stop-WebAppPool -Name "WebAppPool"
                Start-WebAppPool -Name "WebAppPool"
                '''
            }
        }
    }
    post {
        success {
            echo 'Deployment completed successfully'
        }
        failure {
            echo 'Deployment failed'
        }
    }
}

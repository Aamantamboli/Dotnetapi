pipeline {
    agent {
        label 'windows' // Specify your Windows agent label here
    }
    stages {
        stage('Checkout') {
            steps {
                // Checkout the code from GitHub
                git url: 'https://github.com/Aamantamboli/Dotnetapi.git'
            }
        }
        stage('Publish') {
            steps {
                script {
                    // Ensure the correct path to the project file
                    bat 'dotnet publish "KubernetesAutoClusterAPI/*" -c Release -r win-x64 --self-contained -o "C:/inetpub/wwwroot/Dotnetapi"'
                }
            }
        }
    }
    post {
        success {
            echo 'Publish completed successfully!'
        }
        failure {
            echo 'Publish failed!'
        }
    }
}

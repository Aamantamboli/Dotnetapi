pipeline {
    agent { label 'windows-node' }
    stages {
        stage('Checkout Code') {
            steps {
                // Use the credentialsId from Jenkins to access the Git repository
                git branch: 'main', url: 'https://github.com/Aamantamboli/Dotnetapi.git', credentialsId: 'windows'
            }
        }
    }
}

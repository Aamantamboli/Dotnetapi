pipeline {
    agent { label 'windows' }
    stages {
        stage('Checkout Code') {
            steps {
                // Use the credentialsId from Jenkins to access the Git repository
                git branch: 'master', url: 'https://github.com/Aamantamboli/Dotnetapi.git', credentialsId: 'windows'
            }
        }
    }
}

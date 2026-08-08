pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore TravelFinderApi/TravelFinderApi.csproj'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build TravelFinderApi/TravelFinderApi.csproj --configuration Release --no-restore'
            }
        }
    }
}
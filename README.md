# Global Logistics Management System (GLMS)

## Overview

The Global Logistics Management System (GLMS) is an ASP.NET Core MVC enterprise application developed to manage logistics clients, contracts and service requests within a centralized platform.

The system was designed for TechMove Logistics to replace fragmented spreadsheet-based processes with a structured web-based solution that improves workflow management, document handling and operational visibility.

## Features

### Client Management
- Create, view, edit and delete clients
- Store client contact details and regional information

### Contract Management
- Create, view, edit and delete contracts
- Track contract status (Draft, Active, Expired and On Hold)
- Associate contracts with clients
- Upload and download signed PDF agreements

### Service Request Management
- Create, view, edit and delete service requests
- Link service requests to contracts
- Track request statuses and costs

### Workflow Validation
- Prevent creation of Service Requests for Expired contracts
- Prevent creation of Service Requests for On Hold contracts
- Validate user input before saving data

### Search and Filtering
- Filter contracts by status
- Filter contracts by date range

### Currency Conversion
- Convert USD amounts to ZAR using an external Currency Exchange API
- Automatically calculate and store local currency values

### Unit Testing
- Currency Conversion Validation
- PDF File Validation
- EXE File Rejection Validation
- USD Amount Validation
- Expired Contract Validation
- On Hold Contract Validation

## Technologies Used

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- Bootstrap
- xUnit
- GitHub
- Currency Exchange API

## Database Structure

### Client
- ClientId
- Name
- ContactDetails
- Region

### Contract
- ContractId
- ClientId
- StartDate
- EndDate
- Status
- ServiceLevel
- AgreementFilePath

### ServiceRequest
- ServiceRequestId
- ContractId
- Description
- UsdAmount
- Cost
- Status

## Design Patterns Implemented

### Singleton Pattern
The ApplicationSettingsService is implemented as a Singleton and registered using AddSingleton() to ensure that only one shared instance exists throughout the application's lifecycle.

### Factory Pattern
The ServiceRequestFactory is used to centralize object creation for Service Requests, improving maintainability and separation of concerns.

## Business Rules

- Service Requests cannot be created for Expired contracts.
- Service Requests cannot be created for On Hold contracts.
- Only PDF files may be uploaded as Signed Agreement documents.
- USD Amount must be greater than zero.

## Unit Tests Implemented

The project includes tests for:
- Currency Conversion
- PDF Acceptance
- EXE File Rejection
- USD Amount Validation
- Expired Contract Validation
- On Hold Contract Validation

## Developer

Tshenolo Millicent Mogane

Enterprise Application Development (EAPD7111)

Programming 3A (PROG7311)

#!/bin/bash
set -e

EchoInColor()
{
	str="$1"
	color="$2"
	if [[ "$color" == "" ]]; then
		color="0;32"
	fi
	color="\033[${color}m"

	if [[ "$ENABLE_ANSI" == "true" ]]; then
		echo -e "${color}${str}\033[0m"
	else
		echo -e "${str}"
	fi
}

ProgressStart()
{
	if [[ "${SHOW_EXTRA_INFO}" != "true" ]]; then
		return
	fi

	EchoInColor "$(date '+%Y-%m-%d %H:%M:%S') - Starting: $1..." "0;32"
}

ProgressEnd()
{
	if [[ "${SHOW_EXTRA_INFO}" != "true" ]]; then
		return
	fi

	EchoInColor "$(date '+%Y-%m-%d %H:%M:%S') - Finished: $1" "0;32"
	echo -e ""
}

GetFirstFile()
{
	while [[ $# -gt 0 ]]
	do
		local filePath="$1"
		if [[ -f "$filePath" ]]; then
			echo "$filePath"
			return
		fi
		shift
	done

	echo ""
	return
}

Initialize()
{
	local progressName="Initialize"
	ProgressStart "$progressName"

	SlnFile="MediaTester.sln"

	if [[ "$IS_BUILD" == "true" ]] && [[ "$MSBuildPath" == "" ]]; then
		# List of MSBuild paths. Preferred paths listed first.
		MSBuildPath=$(GetFirstFile \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" \
					)

		if [[ "${MSBuildPath}" == "" ]]; then
			echo "Could not find MSBuild.exe in any expected paths."
			echo "Ensure MS Build Tools or Visual Studio Professional is installed. (MSBuild 16.* required / VS2019)"
			exit 1
		fi

		echo "Using MSBuild: ${MSBuildPath}"
	fi

	if [[ "$ENABLE_UNIT_TESTS" == "true" ]]; then
		# List of \vstest.console.exe paths. Preferred paths listed first.
		VsTestPath=$(GetFirstFile \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" \
					"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" \
					)

		if [[ "${VsTestPath}" == "" ]]; then
			echo "Could not find vstest.console.exe in any expected paths."
			echo "Ensure MS Build Tools or Visual Studio is installed."
			exit 1
		fi

		echo "Using VsTest: ${VsTestPath}"
	fi

	ProgressEnd "$progressName"
}

RunSolutionTarget()
{
	local slnFile="$1"
	local target="$2"
	local msBuildPath="$3"
	local progressName="${target} solution '${slnFile}'"
	ProgressStart "$progressName"

	# -m for parallel build
	"${msBuildPath}" "${slnFile}" -m -target:${target} -property:Configuration=Release

	ProgressEnd "$progressName"
}
# main
if [ $# -eq 0 ]; then
	echo "No arguments provided!"
	echo "  Use '--all --release' for production release build."
	echo "  Valid arguments:"
	echo "    --all           Build all."
	#echo "                    Unit tests will be run automatically."
	#echo "    --test          Run unit tests without building."
	#echo "                    Release build only."
	#echo "                    Add '-framework' or '-core' for specific targets."
	echo "    --no-clean      Skip the clean step before building."
	echo "                    Unchanged projects will not be rebuilt."
	#echo "    --no-test       Disable running any unit tests."
	echo "    --no-ansi       Do not output ANSI color sequences."
	echo "                    Useful for clean log output or non-ANSI terminals."
	echo "    --log-file      Override default build output log file."
	echo "                    Usage: --log-file [path/filename]"
	echo ""
	exit 1
fi

POSITIONAL=()
dateStr=$(date '+%Y-%m-%d_%H%M%S')
OUTPUT_DIRECTORY="_output/"
mkdir -p "$OUTPUT_DIRECTORY"
BUILD_LOG_FILE="${OUTPUT_DIRECTORY}build-output_${dateStr}.log"
SHOW_EXTRA_INFO=true
#ENABLE_UNIT_TESTS=true
ENABLE_ANSI=true
ENABLE_CLEAN=true
ENABLE_LIST_OUTPUT=true
ORIGINAL_ARGUMENTS="$@"

while [[ $# -gt 0 ]]
do
	key="$1"

	case $key in
		--msbuild-path)
			shift
			MSBuildPath="$1"
			shift
			;;
		--restore)
			RUN_INITIALIZE=true
			IS_BUILD=true
			RESTORE=true
			shift
			;;
		--build)
			RUN_INITIALIZE=true
			LIST_OUTPUT=true
			IS_BUILD=true
			BUILD=true
			shift
			;;
		--clean)
			RUN_INITIALIZE=true
			IS_BUILD=true
			CLEAN=true
			shift
			;;
		#--no-test)
		#	ENABLE_UNIT_TESTS=false
		#	shift
		#	;;
		#--test)
		#	RUN_INITIALIZE=true
		#	RUN_UNIT_TESTS_FRAMEWORK=true
		#	RUN_UNIT_TESTS_CORE=true
		#	shift
		#	;;
		#--test-framework)
		#	RUN_INITIALIZE=true
		#	RUN_UNIT_TESTS_FRAMEWORK=true
		#	shift
		#	;;
		#--test-core)
		#	RUN_INITIALIZE=true
		#	RUN_UNIT_TESTS_CORE=true
		#	shift
		#	;;
		--all)
			RUN_INITIALIZE=true
			LIST_OUTPUT=true
			IS_BUILD=true
			CLEAN=true
			RESTORE=true
			BUILD=true
			#RUN_UNIT_TESTS_FRAMEWORK=true
			#RUN_UNIT_TESTS_CORE=true
			shift
			;;
		--log-file)
			BUILD_LOG_FILE="$2"
			shift 2
			;;
		--release)
			GENERATE_DYNAMIC_VERSION=true
			IS_RELEASE=true
			shift
			;;
		--release-alpha)
			IS_RELEASE=true
			RELEASE_TYPE=alpha
			shift
			;;
		--release-beta)
			IS_RELEASE=true
			RELEASE_TYPE=beta
			shift
			;;
		--unrelease)
			IS_UNRELEASE=true
			shift
			;;
		--generate-dynamic-version)
			GENERATE_DYNAMIC_VERSION=true
			shift
			;;
		--no-clean)
			ENABLE_CLEAN=false
			shift
			;;
		--no-list-output)
			ENABLE_LIST_OUTPUT=false
			;;
		--no-show-extra-info)
			SHOW_EXTRA_INFO=false
			;;
		--no-ansi)
			ENABLE_ANSI=false
			shift
			;;
		--automation-mode)
			SHOW_EXTRA_INFO=false
			ENABLE_LIST_OUTPUT=false
			ENABLE_ANSI=false
			BUILD_LOG_FILE=/dev/null
			shift
			;;
		*)    # unknown option
			POSITIONAL+=("$1") # save it in an array for later
			shift
			;;
	esac
done
set -- "${POSITIONAL[@]}" # restore positional parameters

if [[ "${SHOW_EXTRA_INFO}" == "true" ]]; then
	echo -e "Logging build output to: ${BUILD_LOG_FILE}"
	echo ""
fi

{
	set -e
	echo "Arguments: ${ORIGINAL_ARGUMENTS}"
	echo ""

	if [[ "$RUN_INITIALIZE" == "true" ]]; then
		Initialize
	fi

	if [[ "$ENABLE_CLEAN" == "true" ]] && [[ "$CLEAN" == "true" ]]; then
		RunSolutionTarget "${SlnFile}" "Clean" "${MSBuildPath}"
	fi

	if [[ "$RESTORE" == "true" ]]; then
		RunSolutionTarget "${SlnFile}" "Restore" "${MSBuildPath}"
	fi

	if [[ "$BUILD" == "true" ]]; then
		RunSolutionTarget "${SlnFile}" "Build" "${MSBuildPath}"
		
		mkdir -p ${OUTPUT_DIRECTORY}MediaTesterGui
		mkdir -p ${OUTPUT_DIRECTORY}MediaTesterCli
		mkdir -p ${OUTPUT_DIRECTORY}MediaTesterLib
		
		rm -f ${OUTPUT_DIRECTORY}MediaTesterGui/*
		rm -f ${OUTPUT_DIRECTORY}MediaTesterCli/*
		rm -f ${OUTPUT_DIRECTORY}MediaTesterLib/*
		
		cp MediaTesterGui/bin/Release/net462/MediaTesterGui.exe ${OUTPUT_DIRECTORY}MediaTesterGui/
		cp MediaTesterCli/bin/Release/net462/MediaTester.exe ${OUTPUT_DIRECTORY}MediaTesterCli/
		cp MediaTesterLib/bin/Release/netstandard2.0/*.dll ${OUTPUT_DIRECTORY}MediaTesterLib/
	fi

	#if [[ "$ENABLE_UNIT_TESTS" == "true" ]]; then
	#	if [[ "$RUN_UNIT_TESTS_FRAMEWORK" == "true" ]]; then
	#		TestFramework
	#	fi
	#	if [[ "$RUN_UNIT_TESTS_CORE" == "true" ]]; then
	#		TestCore
	#	fi
	#fi

	if [[ "$IS_UNRELEASE" == "true" ]]; then
		UnPrepareProjectsForRelease
	fi

	if [[ "${SHOW_EXTRA_INFO}" == "true" ]]; then
		echo ""
		EchoInColor "Build complete!" "1;32"
	fi

	if [[ "$LIST_OUTPUT" == "true" ]] && [[ "$ENABLE_LIST_OUTPUT" == "true" ]]; then
		EchoInColor "Build output can be found in the '${OUTPUT_DIRECTORY}' directory:" "1;32"
		if [ -d "${OUTPUT_DIRECTORY}" ]; then
			echo ""
			EchoInColor "${OUTPUT_DIRECTORY}:"
			ls -hoRg ${OUTPUT_DIRECTORY}*/*
		fi
	fi
} 2>&1 | tee "${BUILD_LOG_FILE}"

ExitCode=${PIPESTATUS[0]}

if [[ "${SHOW_EXTRA_INFO}" == "true" ]]; then
	echo ""
	EchoInColor "Build output logged to: ${BUILD_LOG_FILE}"
fi

if [[ "$ExitCode" != "0" ]]; then
	echo ""
	echo "Errors detected!"
fi

exit $ExitCode




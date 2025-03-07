#! /usr/bin/bash
cd "$(dirname "$0")"

get_csprojs() {
    csproj_pattern='Project\(.*\)\s*=\s*\"[^"]+\"\s*,\s*\"([^"]+\.csproj)\"'
    sed -nE "s/.*$csproj_pattern.*/\1/p" ../../src/FoodDeliverySystem.sln | sed 's/\\/\//g'
}

get_folder_path() {
    if (( $# != 2 )); then
        >&2 echo "Script error"
        return 1
    fi

    IFS='/'; local split=($1); unset IFS;
    local split_length="${#split[@]}"
    local folder_len=$(($split_length - $2))
    echo $(IFS='/'; echo "${split[*]:0:$folder_len}")
}

get_base_folder_path() {
    if (( $# != 2 )); then
        >&2 echo "Script error"
        return 1
    fi

    IFS='/'; local split=($1); unset IFS;
    echo $(IFS='/'; echo "${split[*]:0:$2}")
}

escape_special_chars() {
    if (( $# != 1 )); then
        >&2 echo "Script error"
        return 1
    fi

    echo "$1" | sed 's/[\/&]/\\&/g'
}

source ./.env
if (( "${#API_PROJECTS[@]}" == 0 )) || [[ -z $API_PROJECTS ]]; then
    echo "No project configured!"
    echo "Exiting generation."
    exit
fi

all_projects=$(get_csprojs)

declare -A grouped_projects
while read -r project_path; do
    solution_folder_path=$(get_folder_path $project_path 2) || exit $?
    grouped_projects[$solution_folder_path]+="$project_path;"
done <<< $all_projects

restore_copy_commands=

IFS=$'\n' keys=($(sort <<<"${!grouped_projects[*]}"))
unset IFS

for key in "${keys[@]}"; do
    value="${grouped_projects[$key]}"
    IFS=';'; split=($value); unset IFS;

    for proj in "${split[@]}"; do
        folder_path=$(get_folder_path $proj 1) || exit $?
        restore_copy_commands+="COPY [\"$proj\", \"$folder_path/\"]
"
    done

    restore_copy_commands+="
"
done

restore_copy_commands=$(escape_special_chars "$restore_copy_commands")

for api_project in "${API_PROJECTS[@]}"; do

    echo "Processing $api_project"

    project_path="/src/$(get_folder_path $api_project 1)" || exit $?
    source_path=$(get_base_folder_path $api_project 2) || exit $?
    source_copy="COPY [\"$source_path\", \"$source_path\"]"
    project_file=$(basename $api_project)
    dll_file="$(basename $api_project .csproj).dll"

    perl -0777 -pe "s/{{SOURCE_COPY}}/$(escape_special_chars "$source_copy")/g;
                    s/{{BUILD_WORKDIR}}/\"$(escape_special_chars "$project_path")\"/g;
                    s/{{PROJECT_FILE}}/\"$project_file\"/g;
                    s/{{DLL_FILE}}/\"$dll_file\"/g;
                    s/{{RESTORE_PROJECTS}}/$restore_copy_commands/g;" template.txt > "../..$project_path/Dockerfile"

done

echo "Done"
exit 0
